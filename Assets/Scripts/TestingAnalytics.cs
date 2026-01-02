using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class TestingAnalytics : MonoBehaviour
{
    public static TestingAnalytics Instance { get; private set; }

    private List<TestObject> coffeeList;
    private List<TestObject> dartList;
    private List<TestObject> printerList;
    private List<TestObject> cleaningList;
    public List<List<TestObject>> allLists { get; private set; }


    //private Dictionary<taskEnum, float> taskTimes;
    //private Dictionary<taskEnum, int> taskPlays;
    //private Dictionary<taskEnum, int> taskClicks;
    //private taskEnum[] allTasks;

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

    private void Start()
    {
        allLists = new List<List<TestObject>>();
        coffeeList = new List<TestObject>();
        dartList = new List<TestObject>();
        printerList = new List<TestObject>();
        cleaningList = new List<TestObject>();
        allLists.Add(coffeeList);
        allLists.Add(dartList);
        allLists.Add(printerList);
        allLists.Add(cleaningList);
    }

    public void RecordMinigameData(taskEnum taskEnum, float time, int clicks, bool accomplished)
    {
        switch (taskEnum)
        {
            case taskEnum.Dart:
                dartList.Add(new TestObject(taskEnum, time, clicks, accomplished));
                break;
            case taskEnum.Coffee:
                coffeeList.Add(new TestObject(taskEnum, time, clicks, accomplished));
                break;
            case taskEnum.Printer:
                printerList.Add(new TestObject(taskEnum, time, clicks, accomplished));
                break;
            case taskEnum.Mopping:
                cleaningList.Add(new TestObject(taskEnum, time, clicks, accomplished));
                break;
        }
    }

    //void Start()
    //{
    //    taskTimes = new Dictionary<taskEnum, float>();
    //    taskPlays = new Dictionary<taskEnum, int>();
    //    taskClicks = new Dictionary<taskEnum, int>();

    //    allTasks = (taskEnum[])System.Enum.GetValues(typeof(taskEnum));

    //    foreach (taskEnum task in allTasks)
    //    {
    //        taskTimes[task] = 0f;
    //        taskPlays[task] = 0;
    //        taskClicks[task] = 0;
    //    }

    //}

    //public void LogIfItsBeenPlayed(taskEnum taskId)
    //{
    //    taskPlays[taskId] ++;
    //}

    //public void LogHowLongBeenPlayed(taskEnum taskId, float time)
    //{
    //    taskTimes[taskId] += time;
    //}

    //public void LogHowManyClicks(taskEnum taskId, int clicks)
    //{
    //    taskClicks[taskId] += clicks;
    //}

    //public float CalculateAverageClicks(taskEnum taskId)
    //{
    //    if (taskPlays[taskId] == 0) return 0f;
    //    return taskClicks[taskId] / taskPlays[taskId];
    //}

    //public float CalculateAverageTimeInSeconds(taskEnum taskId)
    //{
    //    if (taskPlays[taskId] == 0) return 0f;
    //    return taskTimes[taskId] / taskPlays[taskId];
    //}

    //public int ReturnTimesPlayed(taskEnum taskId)
    //{
    //    return taskPlays[taskId];
    //}

    //public void CreateTestData()
    //{
    //    foreach (taskEnum task in allTasks)
    //    {
    //        taskTimes[task] = Random.Range(30f, 180f);
    //        taskPlays[task] = Random.Range(2, 12);
    //        taskClicks[task] = Random.Range(20, 200);
    //    }
    //}
}

public class TestObject
{
    public taskEnum taskEnum { get; set; }
    public float time { get; set; }
    public int clicks { get; set; }
    public bool accomplished { get; set; }
    public TestObject(taskEnum taskEnum, float time, int clicks, bool accomplished)
    {
        this.taskEnum = taskEnum;
        this.time = time;
        this.clicks = clicks;
        this.accomplished = accomplished;
    }
}
