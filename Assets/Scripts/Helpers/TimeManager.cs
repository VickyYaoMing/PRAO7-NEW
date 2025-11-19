using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections.Generic;
using System;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }
    private TimerEasy dayTimer;
    public Dictionary<GameObject, TimerEasy> taskTimers;

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
        taskTimers = new Dictionary<GameObject, TimerEasy>();
        dayTimer = new TimerEasy(GameData.DayTimer);
    }

    private void OnEnable()
    {
        TaskGiver.AddTaskTimer += AddTimer; 
    }
    private void OnDisable()
    {
        TaskGiver.AddTaskTimer -= AddTimer;
    }
    public void AddTimer(GameObject npc)
    {
        var timer = new TimerEasy(GameData.TaskTimer);

        timer.OnHalfwayReached += () =>
        {
            var giver = npc.GetComponent<TaskGiver>();
            if (giver != null) giver.TaskIsYellow();
        };

        timer.OnAlmostDoneReached += () =>
        {
            var giver = npc.GetComponent<TaskGiver>();
            if (giver != null) giver.TaskIsRed();
        };

        taskTimers[npc] = timer;
    }

    private void Update()
    {
        dayTimer.UpdateTimer(Time.deltaTime);

        if (dayTimer.currentState == TimerEasy.TimerEnum.isTimerDone)
        {
            Destroy(gameObject);
            SceneManager.LoadScene(StringLiterals.GAMEOVER_SCENE);
        }
        if (taskTimers.Count <= 0) return;

        foreach (var pair in taskTimers)
        {
            pair.Value.UpdateTimer(Time.deltaTime);
        }
    }
    public void ResetTimer()
    {
        dayTimer = new TimerEasy(GameData.DayTimer);
    }
    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

}
