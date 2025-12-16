using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class TestingAnalytics : MonoBehaviour
{
    public static TestingAnalytics Instance { get; private set; }

    private Dictionary<taskEnum, float> taskTimes;
    private Dictionary<taskEnum, int> taskPlays;
    private Dictionary<taskEnum, int> taskClicks;
    private taskEnum[] allTasks;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        taskTimes = new Dictionary<taskEnum, float>();
        taskPlays = new Dictionary<taskEnum, int>();
        taskClicks = new Dictionary<taskEnum, int>();

        allTasks = (taskEnum[])System.Enum.GetValues(typeof(taskEnum));

        foreach (taskEnum task in allTasks)
        {
            taskTimes[task] = 0f;
            taskPlays[task] = 0;
            taskClicks[task] = 0;
        }

    }
   
    public void LogIfItsBeenPlayed(taskEnum taskId)
    {
        taskPlays[taskId] ++;
    }

    public void LogHowLongBeenPlayed(taskEnum taskId, float time)
    {
        taskTimes[taskId] += time;
    }

    public void LogHowManyClicks(taskEnum taskId, int clicks)
    {
        taskClicks[taskId] += clicks;
    }

    public float CalculateAverageClicks(taskEnum taskId)
    {
        if (taskPlays[taskId] == 0) return 0f;
        return taskClicks[taskId] / taskPlays[taskId];
    }

    public float CalculateAverageTimeInSeconds(taskEnum taskId)
    {
        if (taskPlays[taskId] == 0) return 0f;
        return taskTimes[taskId] / taskPlays[taskId];
    }

    public int ReturnTimesPlayed(taskEnum taskId)
    {
        return taskPlays[taskId];
    }

    public void CreateTestData()
    {
        foreach (taskEnum task in allTasks)
        {
            taskTimes[task] = Random.Range(30f, 180f);
            taskPlays[task] = Random.Range(2, 12);
            taskClicks[task] = Random.Range(20, 200);
        }
    }


}


