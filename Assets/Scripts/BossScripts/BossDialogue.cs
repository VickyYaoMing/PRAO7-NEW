using UnityEngine;
using System;

public class BossDialogue : MonoBehaviour
{
    //private System.Random randomTaskPicker;
    //private taskIDEnum currentTask;
    //private int totalTasks;
    //private Boss bossClass;
    //private TaskEventArgs currentPlayerTaskProgress;
    //public delegate bool checkForPlayerHandItem(taskIDEnum taskItem);
    //public static event checkForPlayerHandItem checkForPlayerItem;
    //void Start()
    //{
    //    randomTaskPicker = new System.Random();
    //    totalTasks = Enum.GetValues(typeof(taskIDEnum)).Length;
    //    //currentTask = (taskIDEnum)randomTaskPicker.Next(totalTasks);
    //    currentTask = taskIDEnum.VendingMachine;
    //    bossClass = GetComponent<Boss>();
    //    bossClass.AddTask((int)currentTask, 20f);
    //    Debug.Log(currentTask.ToString());
    //}

    //private void OnEnable()
    //{
    //    if(TaskInteraction.currentTaskInfo != null) currentPlayerTaskProgress = TaskInteraction.currentTaskInfo;
    //    if(currentPlayerTaskProgress != null && currentPlayerTaskProgress.taskId == currentTask && currentPlayerTaskProgress.taskSucceeded && !currentPlayerTaskProgress.hasReturnItem)
    //    {
    //        OnTaskAchieved();
    //    }
    //    TaskInteraction.currentTaskAchieved += OnTaskAchieved;
    //    TaskInteraction.getCurrentTaskFromBoss += ReturnCurrentTask;
    //}
    //private void OnDisable()
    //{
    //    TaskInteraction.currentTaskAchieved -= OnTaskAchieved;
    //    TaskInteraction.getCurrentTaskFromBoss -= ReturnCurrentTask;
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(StringLiterals.PLAYER_TAG))
        {
            //bool hasCorrectItem = checkForPlayerItem.Invoke(currentTask);
            // if (hasCorrectItem) OnTaskAchieved();

            //CheckHandItem();
            Boss.Instance.CheckItemTasks();
        }
    }

    //private taskIDEnum ReturnCurrentTask()
    //{
    //    return currentTask;
    //}

    //private void OnTaskAchieved()
    //{
    //    bossClass.TaskSucceeded((int)currentTask);
    //    Debug.Log("Mission accomplished");
    //    currentTask = (taskIDEnum)randomTaskPicker.Next(totalTasks);
    //    bossClass.AddTask((int)currentTask, 20f);
    //    Debug.Log(currentTask.ToString());
    //}

    //private void CheckHandItem()
    //{
    //    var im = FindAnyObjectByType<InteractionManager>();

    //    foreach(var task in Boss.Instance.Tasks)
    //    {
    //        if (task.id == (int)taskIDEnum.Mopping)
    //            continue;

    //        if (task.id == (int)taskIDEnum.VendingMachine)
    //        {
    //            foreach(var desiredItem in VendingMachine.desiredItems)
    //            {
    //                if (im.HandItemName().Contains(desiredItem) || desiredItem.Contains(im.HandItemName()))
    //                {
    //                    Boss.Instance.TaskSucceeded(task.id);
    //                    im.RemoveItemInHand(null, null);
    //                    break;
    //                }
    //            }
    //            continue;
    //        }

    //        if (im.HandItemName() == im.ItemName(task.id))
    //        {
    //            Boss.Instance.TaskSucceeded(task.id);
    //            im.RemoveItemInHand(null, null);
    //            break;
    //        }
    //    }
    //}
}
public enum taskIDEnum { VendingMachine = 0, Printer = 1, Coffee = 2, Mopping = 3}
