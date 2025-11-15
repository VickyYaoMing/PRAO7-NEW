using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    [SerializeField] public List<HandHeldItem> allHandHeldItems;
    [SerializeField] private GameObject iconSpawn;

    public int maximumAmountOfTasksThatCanBeActive { get; set; } = 3;

    private Task[] currentActiveTasks;
    private Task[] allTasks;
    private int currentIndex = 0;
    private int amountOfActiveTasks;
    private TimerEasy timeBetweenAddingTasks;

    public static event Action<Task> TaskAdded;
    public static event Action<Task> TaskRemoved;

    private void Start()
    {
        iconSpawn.SetActive(false);
        currentActiveTasks = new Task[maximumAmountOfTasksThatCanBeActive];
        allTasks = new Task[allHandHeldItems.Count];
        CreateAllTasks();
        AddTaskToActive();

        timeBetweenAddingTasks = new TimerEasy(7f);
        timeBetweenAddingTasks.ResetTimer();
    }

    private void OnEnable()
    {
        InteractionManager.missionAccomplished += TaskAccomplished;
    }

    private void OnDisable()
    {
        InteractionManager.missionAccomplished += TaskAccomplished;
    }
    private void AddTaskToActive()
    {
        if (amountOfActiveTasks >= maximumAmountOfTasksThatCanBeActive ) return;

        Task task = null;

        const int maxTries = 20;
        int tries = 0;

        while (tries < maxTries)
        {
            task = allTasks[UnityEngine.Random.Range(0, allTasks.Length)];
            bool alreadyActive = false;

            for (int i = 0; i < currentIndex; i++)
            {
                if (currentActiveTasks[i] != null & currentActiveTasks[i].task == task.task)
                {
                    alreadyActive = true;
                    break;
                }
            }
            if (!alreadyActive) break;

            tries++;
        }

        task.timer.ResetTimer();                     
        currentActiveTasks[currentIndex] = task;
        currentIndex++;

        SpawnIcon(task.returnItem.spriteIcon); 
        TaskAdded?.Invoke(task);
        amountOfActiveTasks++;
    }


    private void Update()
    {
        timeBetweenAddingTasks.UpdateTimer(Time.deltaTime);
        if (currentIndex < currentActiveTasks.Length && timeBetweenAddingTasks.isTimerDone)
        {
            AddTaskToActive();
            timeBetweenAddingTasks.ResetTimer();
        }

        for (int i = 0; i < currentIndex; i++)
        {
            Task task = currentActiveTasks[i];
            if (task == null) continue;
            task.timer.UpdateTimer(Time.deltaTime);
            if (task.timer.isTimerDone)
            {
                TaskRemoved?.Invoke(task);
                RemoveTaskAtIndex(i);
                i--;
                amountOfActiveTasks--;
            }
        }
    }
    public bool TaskAccomplished(taskEnum task)
    {
        for(int i = 0; i < currentIndex; i++)
        {
            Task taskItem = currentActiveTasks[i];
            if (taskItem == null) continue;
            if (task == taskItem.task)
            {
                TaskRemoved?.Invoke(taskItem);
                RemoveTaskAtIndex(i);
                i--;
                amountOfActiveTasks--;
                return true;
            }
        }
        return false;
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

    private void CreateAllTasks()
    {
        for (int i = 0; i < allHandHeldItems.Count; i++)
        {
            Task task = new Task();
            HandHeldItem item = allHandHeldItems[UnityEngine.Random.Range(0, allHandHeldItems.Count)];
            task.task = item.task;
            TimerEasy timer = new TimerEasy(10f);
            task.timer = timer;
            task.returnItem = item;
            allTasks[i] = task;
        }
    }
    private void SpawnIcon(Sprite icon)
    {
        Image img = iconSpawn.GetComponent<Image>();
        img.sprite = icon;

        iconSpawn.SetActive(true);
    }
}
public class Task
{
    public taskEnum task;
    public TimerEasy timer;
    public HandHeldItem returnItem;
}
public enum taskEnum { VendingMachine, Printer, Coffee, Mopping }
public enum HandHeldItemEnum { Milk, Chips, Sandwich, EnergyDrink, Pills, Water, Donut, JellyBean, Coffee, Paper, None }


