using System;
using UnityEngine;

public abstract class StationBase : MonoBehaviour
{
    [SerializeField] protected GameObject keyToEnterStation = null;
    public static event EventHandler<Action> stationEntered;
    [SerializeField] private bool isKeyNeeded = true;
    [SerializeField] public static Func<GameObject> playerKey;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(StringLiterals.PLAYER_TAG))
        {
            if(isKeyNeeded)
            {
                bool isKeyValid = playerKey?.Invoke() == keyToEnterStation;
                if(isKeyValid) stationEntered?.Invoke(this, OnInteract);
            }
            else
            {
                stationEntered?.Invoke(this, OnInteract);
            }
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

