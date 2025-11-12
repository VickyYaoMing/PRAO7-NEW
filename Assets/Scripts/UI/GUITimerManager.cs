using System.Collections.Generic;
using UnityEngine;

public class GUITimerManager : MonoBehaviour
{
    [SerializeField] private GameObject bossObject;

    [SerializeField] private List<GameObject> TaskObjects;
    //private HashSet<Task> taskBuffer;

    private Boss boss;

    private void Start()
    {
        boss = bossObject.GetComponent<Boss>();

        if (TaskObjects == null)
        {
            TaskObjects = new List<GameObject>();
        }

        if (boss != null)
        {
            boss.TaskAdded += OnTaskAdded;
            boss.TaskRemoved += OnTaskRemoved;
        }

        //if (taskBuffer == null)
        //{
        //    taskBuffer = new HashSet<Task>();
        //}

    }

    // REWORK - THIS IS HARDCODED AND I NEED TO WAIT FOR ADDITIONAL IDENTIFIERS I THINK BUT THE TIMER IS ALMOST DONE 
    private void OnTaskAdded(Task task)
    {
        //if (task.id == 1)
        //{
        //    TaskObjects[0].GetComponentInChildren<GUITimer>().Initialize(task);
        //}
        //if (task.id == 2)
        //{
        //    TaskObjects[1].GetComponentInChildren<GUITimer>().Initialize(task);
        //}
        foreach(var obj in TaskObjects)
        {
            var gui = obj.GetComponentInChildren<GUITimer>();
            if (gui.taskStationId == task.id)
            {
                gui.Initialize(task);
                break;
            }
        }
    }

    // REWORK - THIS IS HARDCODED AND I NEED TO WAIT FOR ADDITIONAL IDENTIFIERS I THINK BUT THE TIMER IS ALMOST DONE 
    private void OnTaskRemoved(Task task)
    {
        //if (task.id == 1)
        //{
        //    TaskObjects[0].GetComponentInChildren<GUITimer>().OnDestroy();
        //}
        //if (task.id == 2)
        //{
        //    TaskObjects[1].GetComponentInChildren<GUITimer>().OnDestroy();
        //}

        foreach (var obj in TaskObjects)
        {
            var gui = obj.GetComponentInChildren<GUITimer>();
            if (gui.taskStationId == task.id)
            {
                Destroy(gui);
                break;
            }
        }
    }
}
