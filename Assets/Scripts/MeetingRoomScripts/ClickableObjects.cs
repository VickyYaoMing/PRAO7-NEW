using System;
using UnityEngine;
/// <summary>
/// Have a bool that either seet i�bject to leand or not
/// OnMouseDown() has raycast to check if mouse its an item, call to Onlcicked()
/// OnClicked will have differnt actions for each subclass
/// ApplyState() set wanted state for abject depending on hte bool, each subclass objects have different setups
/// get and set bools for subclasses
/// 
/// </summary>
public abstract class ClickableObjects : MonoBehaviour
{
    public static int amountCleaned = 0;
    protected bool isCleaned = false;
    public static Action<int> UpdateAboutCleaning;
    protected virtual void OnMouseDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform == transform)
            {
                OnClicked();
            }
        }
    }
    public abstract void ApplyState();
    protected abstract void OnClicked();
    public void SetCleanedState(bool state)//sets the bool to a state
    {
        isCleaned = state;
    }
    public bool IsCleanedState()
    {
        return isCleaned;
    }

}
