using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ScoreDisplay : MonoBehaviour
{
    private TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMP_Text>(); 
    }

    private void Start()
    {
        text.text = ScoreManager.totalPoints.ToString();
        ScoreManager.Instance.ResetScore();

        

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            if (StringLiterals.MAIN_SCENE == StringLiterals.DAY1_SCENE)
            {
                StringLiterals.MAIN_SCENE = StringLiterals.DAY2_SCENE;
                SceneManager.LoadScene(StringLiterals.MAIN_SCENE);
                return;
            }
            else if (StringLiterals.MAIN_SCENE == StringLiterals.DAY2_SCENE)
            {
                StringLiterals.MAIN_SCENE = StringLiterals.DAY3_SCENE;
                SceneManager.LoadScene(StringLiterals.MAIN_SCENE);
                return;
            }
            else if (StringLiterals.MAIN_SCENE == StringLiterals.DAY3_SCENE)
            {
                StringLiterals.MAIN_SCENE = StringLiterals.DAY1_SCENE;
                SceneManager.LoadScene(StringLiterals.STARTMENU_SCENE);
                return;
            }

        }
    }
}
