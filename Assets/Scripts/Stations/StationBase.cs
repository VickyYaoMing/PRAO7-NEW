using System;
using UnityEngine;

public abstract class StationBase : MonoBehaviour
{
    public static event EventHandler<Action> stationEntered;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(StringLiterals.PLAYER_TAG))
        {
           stationEntered?.Invoke(this, OnInteract);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(StringLiterals.PLAYER_TAG))
        {
           stationEntered?.Invoke(this, null);
        }
    }
    protected abstract void OnInteract();
}

