using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// creata a list of all cliclable objects, serialized field to add new objects as more subclasses are created
/// Call for setupRound in start
/// Creata a temp list to copy all items in clickable list
/// randomize a number betwwen 0 and list.count
/// take that index and add in to playable list, rmove from temp to avoid same object added in
/// foreach item in playable list set bool to not cleaned, call for applysatet to fix logic
/// for each item left in templist set to cleaned and applystate for logic//might not need but for saftey
/// on Check ifAllCleanedIp if amount == itemspersound then set win=true i guess
/// 
/// </summary>

public class SetUpGameScript : TaskBase
{
    ClickableObjects obj;
   

    [SerializeField] List<ClickableObjects> allObjects;
    List<ClickableObjects> playableObjects;
    [SerializeField] int objectsPerRound = 4;



    void Start()
    {
        playableObjects = new List<ClickableObjects>();
        SetupRound();
    }

    private void OnEnable()
    {
        ClickableObjects.UpdateAboutCleaning += CheckIfAllCleanedUp;
    }

    private void OnDisable()
    {
        ClickableObjects.UpdateAboutCleaning -= CheckIfAllCleanedUp;
    }

    private void CheckIfAllCleanedUp(int amount)
    {
        if (amount == objectsPerRound)
        {
            MissionWasAccomplished = true;
            Debug.Log("Yay you won");
            ClickableObjects.amountCleaned = 0;
            Exit(true);
        }
    }
    public void SetupRound()
    {

        List<ClickableObjects> templist = new List<ClickableObjects>(allObjects);
        playableObjects.Clear();

        for (int i = 0; i < objectsPerRound; i++)
        {
            int randIndex = UnityEngine.Random.Range(0, templist.Count);
            obj = templist[randIndex];
            playableObjects.Add(obj);
            templist.RemoveAt(randIndex);

        }
        foreach (ClickableObjects g in playableObjects)
        {

            if (g != null)
            {
                g.SetCleanedState(false);
                g.ApplyState();
                
            }

        }
        foreach (ClickableObjects g in templist)
        {
            g.SetCleanedState(true);
            g.ApplyState();

        }

    }


}
