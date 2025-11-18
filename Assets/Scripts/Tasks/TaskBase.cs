using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class TaskBase : MonoBehaviour
{
    [SerializeField] protected bool hasReturnItem = true;
    [SerializeField] private taskEnum taskID;
    protected bool MissionWasAccomplished = false;

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
    }
    protected virtual void OnMissionAccomplished()
    {
        InteractionManager.Instance.OnRecieveItem(taskID);
    }

    public void Exit(bool unload = false)
    {
        Scene.OnExit += OnExit;
        Scene.Exit(unload);
    }

    private void OnExit()
    {
        Scene.OnExit -= OnExit;
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

