using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class TaskBase : MonoBehaviour
{
    [SerializeField] protected bool hasReturnItem = true;
    [SerializeField] protected taskEnum taskID;
    protected bool MissionWasAccomplished = false;
    private int mouseClicks = 0;

    protected virtual void Update()
    {
        if (MissionWasAccomplished) OnMissionAccomplished();
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Exit();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            MissionWasAccomplished = true;
        }
        if (Input.GetMouseButtonDown(0))
        {
            mouseClicks++;
        }
    }
    protected void OnMissionAccomplished()
    {

        InteractionManager.Instance.OnRecieveItem(taskID);
    }

    public void Exit(bool unload = false)
    {
        TestingAnalytics.Instance.RecordMinigameData(taskID, Time.timeSinceLevelLoad, mouseClicks, MissionWasAccomplished);
        Scene.OnExit += OnExit;
        Scene.Exit(unload);
    }

    private void OnExit()
    {
        Scene.OnExit -= OnExit;
    }

    private void OnEnable()
    {
        mouseClicks = 0;
    }
}

public class TaskEventArgs
{
    public bool taskSucceeded { get; private set; }
    public bool hasReturnItem { get; private set; }
    public taskEnum taskId { get; private set; }

    public TaskEventArgs(bool taskSucceeded, bool hasReturnItem, taskEnum taskId)
    {
        this.taskSucceeded = taskSucceeded;
        this.hasReturnItem = hasReturnItem;
        this.taskId = taskId;
    }
}

