using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrashInteract : StationBase
{
    public static EventHandler throwAwayCurrentItem;
    protected override void OnInteract()
    {
        throwAwayCurrentItem?.Invoke(this, EventArgs.Empty);
    }
}
