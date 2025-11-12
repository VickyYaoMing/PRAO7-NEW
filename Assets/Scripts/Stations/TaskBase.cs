using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class TaskBase : MonoBehaviour
{
    //public static event EventHandler<TaskEventArgs> returnTaskItem;
    [SerializeField] protected GameObject returnItem = null; //to be removed
    [SerializeField] protected bool hasReturnItem = true;
    [SerializeField] private taskIDEnum taskID;
    protected bool MissionWasAccomplished = false;

    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (MissionWasAccomplished)
                OnMissionAccomplished();

            Debug.Log(MissionWasAccomplished ? "Mission accomplished" : "Mission NOT successful");

            Exit();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            MissionWasAccomplished = true;
        }
    }
    protected virtual void OnMissionAccomplished()
    {
        //returnTaskItem?.Invoke(this, new TaskEventArgs(MissionWasAccomplished, hasReturnItem, taskID));
        if (!returnItem)
            return;
        
        InteractionManager.Instance.HoldItem(returnItem);
    }

    public void Exit(bool unload = false)
    {
        Scene.OnExit += OnExit;
        Scene.Exit(unload);
    }

    private void OnExit()
    {
        Scene.OnExit -= OnExit;
        SceneEventHandler.OnMainSceneEnter(this);

        if (!MissionWasAccomplished) return;

        if (this is coffeCup_logic)
            InteractionManager.Instance.HoldItem(Instantiate(ItemDatabase.Instance.coffeeItem.gameObject));
        else if (this is PrinterController)
            InteractionManager.Instance.HoldItem(Instantiate(ItemDatabase.Instance.printerItem.gameObject));
        else if (this is SetUpGameScript)
            Boss.Instance.TaskSucceeded((int)taskIDEnum.Mopping);
    }
}

public class TaskEventArgs
{
    public bool taskSucceeded { get; private set; }
    public bool hasReturnItem { get; private set; }
    public taskIDEnum taskId { get; private set; }

    public TaskEventArgs(bool taskSucceeded, bool hasReturnItem, taskIDEnum taskId)
    {
        this.taskSucceeded = taskSucceeded;
        this.hasReturnItem = hasReturnItem;
        this.taskId = taskId;
    }
}

