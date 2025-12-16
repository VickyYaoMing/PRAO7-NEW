using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSetter : MonoBehaviour
{
    void Awake()
    {
        StringLiterals.MAIN_SCENE = SceneManager.GetActiveScene().name;
    }
}
