using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class WhiteboardUi : MonoBehaviour
{

    [Header("Scene Stuff")]
    private Dictionary<string, string> nextSceneMap = new Dictionary<string, string>
    {
        {StringLiterals.DAY1_SCENE, StringLiterals.DAY2_SCENE },
        {StringLiterals.DAY2_SCENE, StringLiterals.DAY3_SCENE },
        {StringLiterals.DAY3_SCENE, StringLiterals.STARTMENU_SCENE },
    };

    private void Start()
    {
        
    }

    // Change Scene, this is written with the assumption that end game screen does not become main scene
    public void OnButtonPress()
    {
        string currentScene = StringLiterals.MAIN_SCENE;
        if (nextSceneMap.TryGetValue(currentScene, out string nextScene))
        {
            StringLiterals.MAIN_SCENE = nextScene;
            SceneManager.LoadScene(nextScene);
        }
    }
}