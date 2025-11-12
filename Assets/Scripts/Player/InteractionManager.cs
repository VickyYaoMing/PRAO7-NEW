using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using NUnit.Framework;
using UnityEngine;

public class InteractionManager : SavableObject
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
    //public static event EventHandler<PlayerData> onPlayerSaveData;
    private GameObject currentHoldItem = null;
    [SerializeField] private Transform handSlot;
    [SerializeField] private List<GameObject> handHeldItems;
    //private TaskEventArgs currentTaskInfo = null;

    public GameObject CurrentHoldItem => currentHoldItem;

    private void Start()
    {
        instance = this;

        for(int i = 0; i < handHeldItems.Count; i++)
        {
            GameObject gameObject = Instantiate(handHeldItems[i]);
            gameObject.transform.SetParent(handSlot, false);
            gameObject.SetActive(false);
            handHeldItems[i] = gameObject;
        }
    }
    protected void OnEnable()
    {
        //currentTaskInfo = null;
        StationBase.stationEntered += IsTriggered;
        TrashInteract.throwAwayCurrentItem += RemoveItemInHand;
        StationBase.playerKey += ReturnKey;
        //TaskInteraction.updatePlayerAboutHoldItem += OnHoldItem;
        //currentTaskInfo = TaskInteraction.currentTaskInfo;
        //if(currentTaskInfo != null && currentTaskInfo.taskSucceeded) OnHoldItem(currentTaskInfo.taskId);
        //BossDialogue.checkForPlayerItem += CheckIfPlayerHasCorrectItem;

    }
    protected void OnDisable()
    {
        //currentTaskInfo = null;
        StationBase.stationEntered -= IsTriggered;
        StationBase.playerKey -= ReturnKey;
        TrashInteract.throwAwayCurrentItem -= RemoveItemInHand;
        //TaskInteraction.updatePlayerAboutHoldItem -= OnHoldItem;
        //BossDialogue.checkForPlayerItem -= CheckIfPlayerHasCorrectItem;
    }

    //public bool CheckIfPlayerHasCorrectItem(taskIDEnum taskId)
    //{
    //    if(taskId == taskIDEnum.Mopping)
    //    {
    //        return false;
    //    }
    //    else if(handHeldItems[(int)taskId] == currentHoldItem)
    //    {
    //        RemoveItemInHand(this, EventArgs.Empty);
    //        return true;
    //    }
    //    return false;
    //}

    private GameObject ReturnKey()
    {
        return currentHoldItem;
    }

    public void RemoveItemInHand(object o, EventArgs args)
    {
        if (currentHoldItem == null) return;
        //currentHoldItem.SetActive(false);
        Destroy(currentHoldItem);
        currentHoldItem = null;
    }

    private void IsTriggered(object sender, Action e)
    {
        currentStationCallback = e;
    }
    public void OnInteract()
    {
        currentStationCallback?.Invoke();
    }
    //protected override void OnSaveData(object e, EventArgs args)
    //{
    //    onPlayerSaveData?.Invoke(this, new PlayerData(transform.position, currentHoldItem));
    //}

    public void HoldItem(GameObject item)
    {
        if (currentHoldItem)
            Destroy(currentHoldItem);

        currentHoldItem = item;
        item.transform.SetParent(handSlot);
        item.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        var colliders = item.GetComponentsInChildren<Collider>();
        foreach (var collider in colliders)
            collider.enabled = false;
    }

    public string ItemName(int index)
    {
        return handHeldItems[index].name;
    }

    public string HandItemName()
    {
        if (currentHoldItem)
            return currentHoldItem.name;

        return "no item!";
    }
}
