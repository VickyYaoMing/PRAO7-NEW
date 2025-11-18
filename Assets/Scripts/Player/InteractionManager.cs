using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using NUnit.Framework;
using UnityEngine;
using static UnityEditor.Progress;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance
    {
        get
        {
            if (!instance)
                Debug.LogError("No InteractionManager");

            return instance;
        }
    }

    private static InteractionManager instance;


    // Index 0 is Coffee, Index 1 is paper

    [SerializeField] private List<GameObject> handHeldObject;
    [SerializeField] private Transform handSlot;

    private GameObject currentHoldableItem = null;
    private taskEnum currentHoldableTaskId = taskEnum.Mopping;
    private Action currentStationCallback;
    public static Action<taskEnum> returnItemToNpc;


    private void Start()
    {
        instance = this;
        for(int i = 0; i < handHeldObject.Count; i++)
        {
            GameObject obj = Instantiate(handHeldObject[i]);
            obj.SetActive(false);
            handHeldObject[i] = obj;
        }
    }
    protected void OnEnable()
    {
        StationBase.stationEntered += IsTriggered;
        TaskGiver.triggerHasBeenEntered += IsNpcTriggered;
    }
    protected void OnDisable()
    {
        StationBase.stationEntered -= IsTriggered;
        TaskGiver.triggerHasBeenEntered -= IsNpcTriggered;

    }
    private bool IsNpcTriggered(taskEnum task)
    {
        if(currentHoldableItem == null) return false;
        if(task == currentHoldableTaskId)
        {
            currentHoldableItem.SetActive(false);
            currentHoldableItem = null;
            return true;
        }
        else
        {
            return false;
        }

    }
    public void OnRecieveItem(taskEnum task)
    {
        if(task == taskEnum.Printer)
        {
            currentHoldableItem = handHeldObject[1];
            currentHoldableItem.SetActive(true);
            currentHoldableTaskId = task;
            currentHoldableItem.transform.SetParent(handSlot, false);
        }
        else if(task == taskEnum.Coffee)
        {
            currentHoldableItem = handHeldObject[0];
            currentHoldableItem.SetActive(true);
            currentHoldableTaskId = task;
            currentHoldableItem.transform.SetParent(handSlot, false);
        }
        else
        {
            returnItemToNpc.Invoke(task);
        }
    }

    private void IsTriggered(object sender, Action e)
    {
        currentStationCallback = e;
    }
    public void OnInteract()
    {
        currentStationCallback?.Invoke();
    }
  
}
