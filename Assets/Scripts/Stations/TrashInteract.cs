using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrashInteract : StationBase
{
    public static Action throwAwayCurrentItem;
    protected override void OnInteract()
    {
        throwAwayCurrentItem?.Invoke();
    }
}
