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
                Debug.LogError("InteractionManager.Instance is null, guess there is no InteractionManager in the main scene?");

            return instance;
        }
    }

    private static InteractionManager instance;

    private Action currentStationCallback;
    private GameObject currentHoldItem = null;
    [SerializeField] private Transform handSlot;
    [SerializeField] private List<GameObject> handHeldItems;

    private void Start()
    {
        instance = this;
    }

    protected void OnEnable()
    {
        StationBase.stationEntered += IsTriggered;
        StationBase.playerKey += ReturnKey;
        TrashInteract.throwAwayCurrentItem += OnItemDrop;
    }
    protected void OnDisable()
    {
        StationBase.stationEntered -= IsTriggered;
        StationBase.playerKey -= ReturnKey;
        TrashInteract.throwAwayCurrentItem -= OnItemDrop;

    }
    private GameObject ReturnKey()
    {
        return currentHoldItem;
    }

    private void OnItemDrop()
    {
        currentHoldItem.SetActive(false);
        currentHoldItem.transform.SetParent(handSlot, false);
        currentHoldItem = null;
    }
    public void OnRecieveItem(taskEnum task)
    {
        if (task == taskEnum.Mopping) return;

        for(int i = 0; i < handHeldItems.Count; i++)
        {
            if (task == taskEnum.Printer && taskEnum.Printer == handHeldItems[i].GetComponent<HandHeldItem>().task)
            {
                currentHoldItem = handHeldItems[i];
                currentHoldItem.SetActive(true);
                currentHoldItem.transform.SetParent(handSlot, false);

            }
            else if (task == taskEnum.Coffee && taskEnum.Coffee == handHeldItems[i].GetComponent<HandHeldItem>().task)
            {
                currentHoldItem = handHeldItems[i];
                currentHoldItem.SetActive(true);
                currentHoldItem.transform.SetParent(handSlot, false);
            }
        }
    }

    public void OnRecieveVendingMachine(HandHeldItem item)
    {
        for (int i = 0; i < handHeldItems.Count; i++)
        {
            if (item.CurrentHandHeldItem == handHeldItems[i].GetComponent<HandHeldItem>().CurrentHandHeldItem)
            {
                currentHoldItem = handHeldItems[i];
                currentHoldItem.SetActive(true);
                currentHoldItem.transform.SetParent(handSlot, false);
            }
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
