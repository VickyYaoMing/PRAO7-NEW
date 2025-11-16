using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{

    public static TaskManager Instance
    {
        get
        {
            if (!instance)
                Debug.LogError("No taskManager");

            return instance;
        }
    }

    private static TaskManager instance;

    [SerializeField] private GameObject iconSpawn;
    [SerializeField] private List<Task> allTasks;

    public int maximumAmountOfTasksThatCanBeActive { get; set; } = 3;

    private Task[] currentActiveTasks;
    private int currentIndex = 0;
    private int amountOfActiveTasks = 0;
    private TimerEasy timeBetweenAddingTasks;

    public static event Action<Task> TaskAdded;
    public static event Action<Task> TaskRemoved;
    public static event Action<Task> OnTaskAccomplished;


    private void Start()
    {
        instance = this;
        iconSpawn.SetActive(false);
        currentActiveTasks = new Task[maximumAmountOfTasksThatCanBeActive];
        AddTaskToActive();

        timeBetweenAddingTasks = new TimerEasy(7f);
        timeBetweenAddingTasks.ResetTimer();
    }
    private void AddTaskToActive()
    {
        if (amountOfActiveTasks >= maximumAmountOfTasksThatCanBeActive ) return;

        Task task = allTasks[UnityEngine.Random.Range(0, allTasks.Count)];
        currentActiveTasks[currentIndex] = task;
        currentIndex++;
        SpawnIcon(task.sprite); 
        TaskAdded?.Invoke(task);
        amountOfActiveTasks++;
        Debug.Log("Task has been added: " + task.task);
    }

    private void Update()
    {
        timeBetweenAddingTasks.UpdateTimer(Time.deltaTime);
        if (currentIndex < currentActiveTasks.Length && timeBetweenAddingTasks.isTimerDone)
        {
            AddTaskToActive();
            timeBetweenAddingTasks.ResetTimer();
        }
    }
    public void TaskAccomplished(taskEnum task)
    {
        for(int i = 0; i < currentIndex; i++)
        {
            Task taskItem = currentActiveTasks[i];
            if (taskItem == null) continue;
            if (task == taskItem.task)
            {
                TaskRemoved?.Invoke(taskItem);
                OnTaskAccomplished?.Invoke(taskItem);
                RemoveTaskAtIndex(i);
                i--;
                amountOfActiveTasks--;
                Debug.Log("Task accomplished " + task);
            }
        }
    }
    private void RemoveTaskAtIndex(int index)
    {
        for (int j = index; j < currentIndex - 1; j++)
        {
            currentActiveTasks[j] = currentActiveTasks[j + 1];
        }
        currentActiveTasks[currentIndex - 1] = null;
        currentIndex--;
    }

    private void SpawnIcon(Sprite icon)
    {
        Image img = iconSpawn.GetComponent<Image>();
        img.sprite = icon;
        iconSpawn.SetActive(true);
    }

}

public enum taskEnum { Dart, Printer, Coffee, Mopping }



