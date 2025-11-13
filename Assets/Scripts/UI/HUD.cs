//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class HUD : MonoBehaviour
//{
//    [SerializeField] TaskHUD taskHUDPrefab;
//    [SerializeField] Transform taskContainer;
//    [SerializeField] Boss boss;

//    Dictionary<Task, TaskHUD> taskHUDs = new();

//    //private void OnEnable()
//    //{
//    //    boss.TaskAdded += OnTaskAdded;
//    //    boss.TaskRemoved += OnTaskRemoved;
//    //}
//    //private void OnDisable()
//    //{
//    //    boss.TaskAdded -= OnTaskAdded;
//    //    boss.TaskRemoved -= OnTaskRemoved;
//    //}

//    private void Start()
//    {
//        boss.TaskAdded += OnTaskAdded;
//        boss.TaskRemoved += OnTaskRemoved;
//    }

//    private void OnDestroy()
//    {
//        boss.TaskAdded -= OnTaskAdded;
//        boss.TaskRemoved -= OnTaskRemoved;
//    }

//    private void OnTaskAdded(Task t)
//    {
//        var taskHUD = Instantiate(taskHUDPrefab, taskContainer);

//        if (t.id == (int)taskIDEnum.VendingMachine)
//        {
//            if (t.requiredItem.sprite)
//                taskHUD.image.sprite = t.requiredItem.sprite;
//            else
//                taskHUD.image.gameObject.SetActive(false);
//        }
//        else if (t.id == (int)taskIDEnum.Coffee)
//            taskHUD.image.sprite = ItemDatabase.Instance.coffeeIcon;
//        else if (t.id == (int)taskIDEnum.Printer)
//            taskHUD.image.sprite = ItemDatabase.Instance.printerIcon;
//        else if (t.id == (int)taskIDEnum.Mopping)
//            taskHUD.image.sprite = ItemDatabase.Instance.moppingIcon;

//        //Debug.Log(taskHUD.gameObject.name);
//        taskHUD.slider.maxValue = t.timer.WaitTime;

//        taskHUDs.Add(t, taskHUD);
//    }

//    private void OnTaskRemoved(Task t)
//    {
//        Destroy(taskHUDs[t].gameObject);
//        taskHUDs.Remove(t);
//    }

//    private void Update()
//    {
//        //foreach(var task in Boss.Instance.Tasks)
//        //{
//        //    if (taskHUDs.ContainsKey(task))
//        //    {
//        //        taskHUDs[task].slider.value = task.timer.TimeLeft;
//        //        taskHUDs[task].slider.fillRect.GetComponent<Image>().color = Color.Lerp(Color.green, Color.red, task.timer.Progress);
//        //    }
//        //}
//    }
//}
