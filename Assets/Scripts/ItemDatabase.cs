//using System.Collections.Generic;
//using NUnit.Framework;
//using UnityEngine;

//public class ItemDatabase : MonoBehaviour
//{




//    public static ItemDatabase Instance
//    {
//        get
//        {
//            if (!instance)
//                Debug.LogError("ItemDatabase.Instance is null, guess there is no ItemDatabase in the main scene?");

//            return instance;
//        }
//    }

//    private static ItemDatabase instance;

//    private void Awake()
//    {
//        instance = this;

//        if (!printerItem)
//            Debug.LogError("ItemDatabase is missing PrinterItem");
//        if (!coffeeItem)
//            Debug.LogError("ItemDatabase is missing PrinterItem");
//        if (vendingMachineItems == null || vendingMachineItems.Length < 1)
//            Debug.LogError("ItemDatabase is missing vendingMachineItems");
//    }

//    [SerializeField] public Item printerItem;
//    [SerializeField] public Item coffeeItem;
//    [SerializeField] public Item[] vendingMachineItems;
//    [SerializeField] public Sprite coffeeIcon;
//    [SerializeField] public Sprite moppingIcon;
//    [SerializeField] public Sprite printerIcon;
//    [SerializeField] public List<Sprite> vendingMachineIcons;
//}
