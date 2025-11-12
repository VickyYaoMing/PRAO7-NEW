using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Boss : MonoBehaviour
{
    [SerializeField] float dayTime = 60f;
    [SerializeField] float minNewTaskTime = 5f;
    [SerializeField] float maxNewTaskTime = 20f;
    [SerializeField] private GameObject iconSpawn;
    [SerializeField] bool multipleTasksPerStation = false;

    public static Boss Instance
    {
        get
        {
            if (!instance)
                Debug.LogError("Boss.Instance is null, guess there is no Boss in the main scene?");

            return instance;
        }
    }

    private static Boss instance;

    public event Action<Task> TaskAdded;
    public event Action<Task> TaskRemoved;
    public event Action<Task> TaskSuccess;
    public event Action<Task> TaskFail;

    public List<Task> Tasks { get; private set; }

    public Timer DayTimer { get; private set; }

    Timer newTaskTimer;
    InteractionManager interactionManager;

    private void Awake()
    {
        iconSpawn.SetActive(false);
        instance = this;

        Tasks = new();

        DayTimer = Timer.Create(dayTime, true, true, false, "Boss - Day Timer");
        DayTimer.Timeout += () =>
        {
            newTaskTimer.Pause();
            Debug.Log("Day ended, do something here!");
        };

        newTaskTimer = Timer.Create(1f, true, true, false, "Boss - New Task Timer");
        newTaskTimer.Timeout += () =>
        {
            int id = -1;
            float time = 40f;

            //int id = 0; // temp, for testing vending machine

            if (multipleTasksPerStation)
            {
                id = Random.Range(0, 4);
            }
            else if (Tasks.Count < 4)
            {
                bool taskExists;

                do
                {
                    id = Random.Range(0, 4);
                    taskExists = false;
                    foreach (var t in Tasks)
                    {
                        if (t.id == id)
                        {
                            taskExists = true;
                            break;
                        }
                    }
                }
                while (taskExists);
            }

            if (id == (int)taskIDEnum.Coffee)
                time += coffeMachineInteraction.brewTime;

            if (id != -1)
                AddTask(id, time);

            float newTaskTime = Random.Range(minNewTaskTime, maxNewTaskTime);
            newTaskTimer.Begin(newTaskTime);
        };

        interactionManager = FindAnyObjectByType<InteractionManager>();
    }

    private void FixedUpdate()
    {
        // quick fix to make mouse unlocked when not in minigame
        if (Cursor.lockState != CursorLockMode.None)
            Cursor.lockState = CursorLockMode.None; 

        //Debug.Log(newTaskTimer.TimeLeft);
        //Debug.Log(DayTimer.TimeLeft);
    }

    private void SpawnIcon(Sprite icon)
    {
        Image img = iconSpawn.GetComponent<Image>();
        img.sprite = icon;

        iconSpawn.SetActive(true);
    }
    public void AddTask(int id, float time)
    {
        Task t = new Task()
        {
            id = id,
            timer = Timer.Create(time)
        };

        t.timer.Timeout += () =>
        {
            TaskFailed(id);
        };

        if (id == (int)taskIDEnum.VendingMachine)
        {
            int vendingItem = Random.Range(0, ItemDatabase.Instance.vendingMachineItems.Length);
            var item = ItemDatabase.Instance.vendingMachineItems[vendingItem];
            t.requiredItem = item;
            SpawnIcon(ItemDatabase.Instance.vendingMachineIcons[vendingItem]);

            Debug.Log($"Boss wants {item} from the vending machine!");
        }
        else if (id == (int)taskIDEnum.Printer)
        {
            t.requiredItem = ItemDatabase.Instance.printerItem;
            SpawnIcon(ItemDatabase.Instance.printerIcon);
        }
        else if (id == (int)taskIDEnum.Coffee)
        {
            t.requiredItem = ItemDatabase.Instance.coffeeItem;
            SpawnIcon(ItemDatabase.Instance.coffeeIcon);
        }
        else
        {
            SpawnIcon(ItemDatabase.Instance.moppingIcon);
        }


        Debug.Log($"{(taskIDEnum)id} task ({id}) added");
        Tasks.Add(t);
        TaskAdded?.Invoke(t);
    }

    public void CheckItemTasks()
    {
        GameObject holdItem = interactionManager.CurrentHoldItem;

        if (!holdItem)
        {
            //Debug.Log("Player not holding any item");
            return;
        }

        bool itemIsValid = interactionManager.CurrentHoldItem.TryGetComponent(out Item item);

        if (!itemIsValid)
        {
            //Debug.Log("Held gameObject is not of type Item");
            return;
        }

        foreach (var t in Tasks)
        {
            if (t.requiredItem == null)
                continue;

            bool correctItem = item.id == t.requiredItem.id;

            //Debug.Log($"Checking item {item} against {t.requiredItem}");

            if (correctItem)
            {
                interactionManager.RemoveItemInHand(null, null);
                Debug.Log($"{(taskIDEnum)t.id} task ({t.id}) success!");
                TaskSuccess?.Invoke(t);
                RemoveTask(t);
                break;
            }

            //if (interactionManager.CurrentHoldItem
            //    && interactionManager.CurrentHoldItem.TryGetComponent(out Item item)
            //    && item.id == t.requiredItem.id)
            //{
            //    interactionManager.RemoveItemInHand(null, null);
            //    Debug.Log($"Task {t.id} success!");
            //    TaskSuccess?.Invoke(t);
            //    RemoveTask(t);
            //    break;
            //}
        }
    }

    public void TaskSucceeded(int id)
    {
        if (!TryGetTask(id, out Task t)) return;
        Debug.Log($"{(taskIDEnum)id} task ({id}) success!");
        TaskSuccess?.Invoke(t);
        RemoveTask(t);
    }

    public void TaskFailed(int id)
    {
        if (!TryGetTask(id, out Task t)) return;
        Debug.Log($"{(taskIDEnum)id} task ({id}) fail!");
        TaskFail?.Invoke(t);
        RemoveTask(t);
    }

    public void RemoveTask(Task t)
    {
        if (t.timer != null)
            Destroy(t.timer);

        //Debug.Log($"Task {t.id} removed");

        Tasks.Remove(t);
        TaskRemoved?.Invoke(t);
    }

    public bool TryGetTask(int id, out Task task)
    {
        task = default;
        bool pred(Task task) => task.id == id;
        if (!Tasks.Exists(pred)) return false;
        task = Tasks.Find((task) => task.id == id);
        return true;
    }
}

public struct Task
{
    public int id;
    public Timer timer;
    public Item requiredItem;
}
