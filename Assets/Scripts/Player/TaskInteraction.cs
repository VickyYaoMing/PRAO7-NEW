using System;
using UnityEngine;

public class TaskInteraction : MonoBehaviour
{
    public static TaskInteraction Instance { get; private set; }
    public static Action currentTaskAchieved;
    public static Action<taskIDEnum> updatePlayerAboutHoldItem;
    public static Func<taskIDEnum> getCurrentTaskFromBoss;
    public static TaskEventArgs currentTaskInfo { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    //private void OnEnable()
    //{
    //    TaskBase.returnTaskItem += OnTaskInteraction;
    //}
    //private void OnDisable()
    //{
    //    TaskBase.returnTaskItem -= OnTaskInteraction;
    //}
    //private void OnTaskInteraction(object e, TaskEventArgs taskEvent)
    //{
    //    currentTaskInfo = taskEvent;
    //}
}
