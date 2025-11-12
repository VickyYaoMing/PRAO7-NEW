using UnityEngine;
using UnityEngine.InputSystem;

public class TestFactory : MonoBehaviour
{
    /*
     * THIS CLASS SHOULD NOT BE FOR THE FINAL GAME BUT IS NEEDED FOR ME TO TEST OUT DIFFERENT TASKS ETC
     * THIS WILL BE REPLACED BY AN AUTOMATIC SYSTEM 
     */



    [SerializeField] Boss boss;

    private void Start()
    {
        
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            boss.AddTask(1, 20);
            Debug.Log("shit worked");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            boss.AddTask(2, 5);
            Debug.Log("shit worked");
        }
        
    }
}
