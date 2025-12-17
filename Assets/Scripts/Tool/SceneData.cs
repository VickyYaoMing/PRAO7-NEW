using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneData : MonoBehaviour
{
    MinigameData currentMinigame;

    void Start()
    {
        currentMinigame = new MinigameData();

        if (gameObject.scene == SceneManager.GetSceneByName("CoffeeScene"))
        {
            currentMinigame.minigameName = "Coffee";
        }
        else if (gameObject.scene == SceneManager.GetSceneByName("DartScene"))
        {
            currentMinigame.minigameName = "Dart";
        }
        else if (gameObject.scene == SceneManager.GetSceneByName("MoppingScene"))
        {
            currentMinigame.minigameName = "Mopping";
        }
        else if (gameObject.scene == SceneManager.GetSceneByName("PrintingScene"))
        {
            currentMinigame.minigameName = "Printing";
        }

    }

    void Update()
    {

        UpdateTimeActive();

        if (!isSceneActive())
        {
            return;
        }
        UpdateClicks();

    }

    private void OnDestroy()
    {
        TelemetryManager.Instance.RegisterMinigame(currentMinigame);
    }

    void UpdateTimeActive()
    {
        currentMinigame.timeActive += Time.deltaTime;
    }

    void UpdateClicks()
    {
        if (Input.GetMouseButtonDown(0))
        {
            currentMinigame.timesClicked++;
        }
    }

    bool isSceneActive()
    {
        return SceneManager.GetActiveScene() == gameObject.scene;
    }

    public MinigameData GetMinigame()
    {
        return currentMinigame;
    }
}

public struct MinigameData
{
    public int minigameId;
    public string minigameName;
    public int timesClicked;
    public float timeActive;
}
