using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Highlight Flicker")]
    public GameObject highlight;
    Image img;
    private float trans = 0.0f;
    private float originColor;
    [SerializeField] private float backgroundColorFlickeringInterval;
    private float flickerTimer;

    [Header("Menu Panels")]
    [SerializeField] GameObject[] menuPanels;
    int activePanelIndex;

    [Header("Menu Items")]
    private TextMeshProUGUI[] menuItems;
    private int selectedIndex;

    void Start()
    {
        img = highlight.GetComponent<Image>();
        originColor = img.color.a;
        flickerTimer = backgroundColorFlickeringInterval;

        activePanelIndex = 0;
        ActivateEmpty(activePanelIndex);
    }

    void Update()
    {
        BackgroundColorFadeSwitch(Time.deltaTime);
    }

    void ActivateEmpty(int index)
    {
        if (menuPanels.Length == 0)
        {
            return;
        }

        for (int i = 0; i < menuPanels.Length; i++)
        {
            menuPanels[i].SetActive(i == index);
        }

        menuItems = menuPanels[index].GetComponentsInChildren<TextMeshProUGUI>();
        selectedIndex = 0;
        MoveHightlight();
    }

    void MoveHightlight()
    {
        if (highlight != null && menuItems.Length > 0)
        {
            highlight.transform.position = menuItems[selectedIndex].transform.position;
        }
    }

    public void OnButtonSignal(string signal)
    {
        switch (signal)
        {
            case "Up":
                if (selectedIndex == 0)
                {
                    break;
                }
                selectedIndex = (selectedIndex - 1 + menuItems.Length) % menuItems.Length;
                MoveHightlight();
                Debug.Log("Up");
                break;
            case "Down":
                if (selectedIndex == menuItems.Length - 1)
                {
                    break;
                }
                selectedIndex = (selectedIndex + 1) % menuItems.Length;
                MoveHightlight();
                Debug.Log("Down");
                break;
            case "Yes":
                OnYesButtonPress();
                break;
            case "No":
                OnNoButtonPress(); 
                break;
        }
    }

    public void OnYesButtonPress()
    {
        string choice = menuItems[selectedIndex].text;
        switch (choice)
        {
            case "Start Game":
                Debug.Log("Start Game Selected");
                StringLiterals.MAIN_SCENE = StringLiterals.DAY1_SCENE;
                SceneManager.LoadScene(StringLiterals.MAIN_SCENE);
                break;
            case "   Exit":
                Application.Quit();
                break;
            default:
                break;
        }
    }

    public void OnNoButtonPress()
    {
        Debug.Log("No");
    }

    void BackgroundColorFadeSwitch(float deltaTime)
    {
        flickerTimer -= 1f * Time.deltaTime;

        if (img.color.a == originColor && flickerTimer <= 0)
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, trans);
            flickerTimer = backgroundColorFlickeringInterval;
        }
        else if (img.color.a == trans && flickerTimer <= 0)
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, originColor);
            flickerTimer = backgroundColorFlickeringInterval;
        }
    }
}
