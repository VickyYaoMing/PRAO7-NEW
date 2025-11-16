using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using NUnit.Framework;
using UnityEngine;
using static UnityEditor.Progress;

public class InteractionManager : MonoBehaviour
{
    private Action currentStationCallback;
    protected void OnEnable()
    {
        StationBase.stationEntered += IsTriggered;
    }
    protected void OnDisable()
    {
        StationBase.stationEntered -= IsTriggered;
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
