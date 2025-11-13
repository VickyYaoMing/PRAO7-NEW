using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// If all you need your station to do is load a specific scene, use this generic station.
/// Make sure the scene is added to the scene list in build settings.
/// And also that this gameobject has a trigger collider that the player can enter to interact.
/// </summary>
public class GenericStation : StationBase
{
    [SerializeField] private string sceneName;

    protected override void OnInteract()
    {
        if (string.IsNullOrEmpty(sceneName))
            return;

        Scene.Enter(sceneName);
    }
}
