using System;
using UnityEngine;

public class StateManagerScript : MonoBehaviour
{
    public static StateManagerScript Instance { get; private set; }
    PlayerData savedPlayerData = new PlayerData(Vector3.zero);
    public PlayerData CurrentData => savedPlayerData;

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
    //    InteractionManager.onPlayerSaveData += OnSavePlayerData;
    //}
    //private void OnDisable()
    //{
    //    InteractionManager.onPlayerSaveData -= OnSavePlayerData;
    //}
    private void OnSavePlayerData(object e, PlayerData playerData)
    {
        savedPlayerData = playerData;
    }
}
public sealed class PlayerData : EventArgs
{
    public Vector3 position { private set; get; }
    public GameObject holdItem { private set; get; }

    public PlayerData(Vector3 position, GameObject holdItem = null)
    {
        this.position = position;
        this.holdItem = holdItem;
    }
}
