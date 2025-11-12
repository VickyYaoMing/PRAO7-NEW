using UnityEngine;
using System.Collections.Generic;

public class PrinterController : TaskBase
{
    [Header("Button References")]
    public List<PrinterButton> printerButtons; 

    [Header("Sequence Settings")]
    public List<Color> correctSequence = new List<Color> { Color.red, Color.yellow, Color.green };

    [Header("Paper Spawning")]
    public GameObject paperPrefab; 
    public Transform paperSpawnPoint;
    public float spawnForce = 5f; 
    public Transform player; 
    public float spawnDistanceThreshold = 10f; // X distance threshold
    public int requiredFarPapersCount = 3;

    private List<GameObject> papers = new List<GameObject>();

    private List<Color> playerSequence = new List<Color>();
    private bool paperInserted = false;
    private bool buttonsActive = false;

    void Start()
    {
        // Make sure collider exists
        if (GetComponent<Collider>() == null)
        {
            gameObject.AddComponent<BoxCollider>();
            GetComponent<BoxCollider>().isTrigger = true;
        }

        // Initially disable buttons
        SetButtonsActive(false);
    }



    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Paper") && !paperInserted)
        {
            PaperInserted(other.gameObject);
        }
    }

    void PaperInserted(GameObject paper)
    {
        paperInserted = true;

        // Destroy the paper
        Destroy(paper);

        // Clear any previous sequence before starting new one
        playerSequence.Clear();

        // Activate and randomize buttons
        ActivateButtons();

        //Debug.Log("Paper inserted! Buttons activated.");
    }

    void ActivateButtons()
    {
        // Create list of colors and shuffle them
        List<Color> colors = new List<Color> { Color.red, Color.yellow, Color.green };
        ShuffleColors(colors);

        // Assign colors to buttons
        for (int i = 0; i < printerButtons.Count && i < colors.Count; i++)
        {
            printerButtons[i].Initialize(colors[i], this);
        }

        SetButtonsActive(true);
    }

    public void ButtonPressed(Color buttonColor)
    {
        if (!buttonsActive) return;

        playerSequence.Add(buttonColor);

        //Debug.Log("Button pressed: " + buttonColor);
        //Debug.Log("Player sequence count: " + playerSequence.Count + ", Correct sequence count: " + correctSequence.Count);

        for (int i = 0; i < playerSequence.Count; i++)
        {
            // safety check
            if (i >= correctSequence.Count)
            {
                //Debug.Log("Sequence too long! Resetting...");
                ResetSequence();
                return;
            }

            if (playerSequence[i] != correctSequence[i])
            {
                // Wrong sequence - reset
                //Debug.Log("Wrong sequence! Resetting...");
                ResetSequence();
                SetButtonsActive(false);
                Invoke(nameof(ActivateButtons), 0.5f);
                return;
            }
        }

        // Check if sequence is complete
        if (playerSequence.Count == correctSequence.Count)
        {
            MissionWasAccomplished = true;
            QuestCompleted();
        }
    }

    void ResetSequence()
    {
        playerSequence.Clear();
        Debug.Log("Sequence reset. Try again!");
    }

    void QuestCompleted()
    {
        Debug.Log("QUEST COMPLETED!");

        // Deactivate buttons
        SetButtonsActive(false);

        // Reset ALL state variables
        playerSequence.Clear();
        paperInserted = false;

        // Spawn new paper
        SpawnNewPaper();

    }

    void SpawnNewPaper()
    {
        if (paperPrefab != null)
        {
            // Determine spawn position
            Vector3 spawnPosition = paperSpawnPoint != null ?
                paperSpawnPoint.position : transform.position + Vector3.up * 1f;

            // Spawn the paper
            GameObject newPaper = Instantiate(paperPrefab, spawnPosition, Quaternion.identity);

            Rigidbody paperRb = newPaper.GetComponent<Rigidbody>();
            if (paperRb != null && spawnForce > 0)
            {
                Vector3 forceDirection = paperSpawnPoint != null ?
                    paperSpawnPoint.forward : Vector3.up;
                paperRb.AddForce(forceDirection * spawnForce, ForceMode.Impulse);
            }

            //Debug.Log("New paper spawned!");
        }
        else
        {
            //Debug.LogWarning("Paper prefab not assigned in PrinterController!");
        }
    }

    void SetButtonsActive(bool active)
    {
        buttonsActive = active;
        foreach (PrinterButton button in printerButtons)
        {
            button.SetActive(active);

            button.gameObject.SetActive(active);
        }

        //Debug.Log("Buttons active: " + active);
    }

    void ShuffleColors(List<Color> colors)
    {
        for (int i = 0; i < colors.Count; i++)
        {
            Color temp = colors[i];
            int randomIndex = Random.Range(i, colors.Count);
            colors[i] = colors[randomIndex];
            colors[randomIndex] = temp;
        }
    }

    public void ResetPrinter()
    {
        playerSequence.Clear();
        paperInserted = false;
        SetButtonsActive(false);
        //Debug.Log("Printer completely reset");
    }
}