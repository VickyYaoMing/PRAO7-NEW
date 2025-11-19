using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class TaskGiver : MonoBehaviour
{
    [SerializeField] private List<Task> availableTasks;
    [SerializeField] private GameObject imageSpawn;
    [SerializeField] private TimerEasy timerTaskSpawn;
    private Task currentTask = null;
    private bool isTaskActive = false;

    public delegate bool CheckIfPlayerHasItem(taskEnum task);
    public static event CheckIfPlayerHasItem triggerHasBeenEntered;
    public static Action<GameObject> AddTaskTimer;
    public static Action<int> taskScore;
    private enum TaskState { Red, Yellow, Green };
    private TaskState currentTaskState = TaskState.Green;

    private void Start()
    {
        imageSpawn.SetActive(false);
        timerTaskSpawn = new TimerEasy(UnityEngine.Random.Range(0, 5));
    }

    private void OnEnable()
    {
        InteractionManager.returnItemToNpc += TaskSucceed;
    }
    private void OnDisable()
    {
        InteractionManager.returnItemToNpc -= TaskSucceed;
    }
    private void SpawnIcon(Sprite icon)
    {
        Image img = imageSpawn.GetComponent<Image>();
        img.sprite = icon;
        imageSpawn.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (currentTask == null) return;
        if (currentTask.task == taskEnum.Dart || currentTask.task == taskEnum.Mopping) return;
        if(other.CompareTag(StringLiterals.PLAYER_TAG))
        {
            if (triggerHasBeenEntered.Invoke(currentTask.task))
            {
                CleanUpTask();
            }
        }
    }

    public void TaskIsYellow()
    {
        if (currentTask == null) return;
        Image img = imageSpawn.GetComponent<Image>();
        img.color = Color.yellow;
        currentTaskState = TaskState.Yellow;
    }

    public void TaskIsRed()
    {
        if (currentTask == null) return;
        Image img = imageSpawn.GetComponent<Image>();
        img.color = Color.red;
        currentTaskState = TaskState.Red;

    }

    private void Update()
    {
        timerTaskSpawn.UpdateTimer(Time.deltaTime);
        if(timerTaskSpawn.currentState == TimerEasy.TimerEnum.isTimerDone && !isTaskActive)
        {
            ActivateTask();
        }
    }

    private void CleanUpTask()
    {
        timerTaskSpawn = new TimerEasy(UnityEngine.Random.Range(2, 5));
        timerTaskSpawn.ResetTimer();
        isTaskActive = false;
        imageSpawn.SetActive(false);
        currentTask = null;
    }
    private void TaskSucceed(taskEnum task)
    {
        if (currentTask == null) return;
        if(task == currentTask.task)
        {
            taskScore?.Invoke(CalculateTaskScore());
            CleanUpTask();
        }
    }
    private void ActivateTask()
    {
        isTaskActive = true;
        currentTask = availableTasks[UnityEngine.Random.Range(0, availableTasks.Count)];
        AddTaskTimer?.Invoke(gameObject);
        SpawnIcon(currentTask.sprite);
        Image img = imageSpawn.GetComponent<Image>();
        img.color = Color.green;
        Debug.Log("Task active: " +  currentTask.task);
        currentTaskState = TaskState.Green;
    }

    private int CalculateTaskScore()
    {
        if(currentTaskState == TaskState.Green)
        {
            return GameData.maxTaskScore;
        }
        else if(currentTaskState == TaskState.Yellow)
        {
            return GameData.yellowTaskScore;
        }
        else
        {
            return GameData.redTaskScore;
        }

    }



}
