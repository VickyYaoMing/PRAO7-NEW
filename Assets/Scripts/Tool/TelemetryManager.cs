using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TelemetryManager : MonoBehaviour
{
    public static TelemetryManager Instance { get; private set; }

    private List<MinigameData> completedMinigames;
    int minigameId;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        minigameId = 0;
    }

    public void RegisterMinigame(MinigameData data)
    {
        minigameId++;
        data.minigameId = minigameId;

        completedMinigames.Add(data);
    }

    public List<MinigameData> GetCompletedMinigames()
    {
        return completedMinigames;
    }

}
