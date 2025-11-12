using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class SavableObject : MonoBehaviour
{
    //protected virtual void OnEnable() => SceneEventHandler.TaskSceneEnter += OnSaveData;
    //protected virtual void OnDisable() => SceneEventHandler.TaskSceneEnter -= OnSaveData;
    //protected abstract void OnSaveData(object e, EventArgs args);

}

public static class SceneEventHandler
{
    public static event EventHandler TaskSceneEnter;
    public static event EventHandler MainSceneEnter;

    public static void OnTaskSceneEnter(object sender) => TaskSceneEnter?.Invoke(sender, EventArgs.Empty);
    public static void OnMainSceneEnter(object sender) => MainSceneEnter?.Invoke(sender, EventArgs.Empty);

}

public static class Scene
{
    public static Dictionary<string, UnityEngine.SceneManagement.Scene> loadedScenes;
    public static Dictionary<string, Dictionary<int, bool>> activeObjects;
    public static event Action OnExit;
    public static bool loading;

    public static async void Enter(string sceneName)
    {
        if (loading) return;
        loading = true;

        if (sceneName == StringLiterals.MAIN_SCENE)
            throw new ArgumentException($"Don't enter the main scene, dummy. That is handled automatically!");

        loadedScenes ??= new();
        activeObjects ??= new();

        GameObject[] rootObjects;

        // enable new scene
        if (loadedScenes.ContainsKey(sceneName))
        {
            rootObjects = loadedScenes[sceneName].GetRootGameObjects();

            foreach (var obj in rootObjects)
                obj.SetActive(activeObjects[sceneName][obj.GetInstanceID()]);
        }
        // load new scene
        else
        {
            await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            loadedScenes[sceneName] = SceneManager.GetSceneByName(sceneName);

            rootObjects = loadedScenes[sceneName].GetRootGameObjects();

            activeObjects[sceneName] = new();
        }

        // disable main scene
        loadedScenes[StringLiterals.MAIN_SCENE] = SceneManager.GetSceneByName(StringLiterals.MAIN_SCENE);
        activeObjects[StringLiterals.MAIN_SCENE] = new();

        rootObjects = loadedScenes[StringLiterals.MAIN_SCENE].GetRootGameObjects();

        foreach (var obj in rootObjects)
        {
            activeObjects[StringLiterals.MAIN_SCENE][obj.GetInstanceID()] = obj.activeSelf;

            // dont disable timers!!
            if (obj.TryGetComponent(out Timer _))
                continue;

            // dont disable Sound Manager!!
            if (obj.TryGetComponent(out SoundManager _))
                continue;

            obj.SetActive(false);
		}

        // set new scene active
        SceneManager.SetActiveScene(loadedScenes[sceneName]);
        loading = false;
    }

    public static void Exit(string sceneName, bool unload = false)
    {
        if (loading) return;
        loading = true;
        if (sceneName == StringLiterals.MAIN_SCENE)
            throw new ArgumentException($"Don't exit the main scene, dummy. That is handled automatically!");

        loadedScenes ??= new();

        if (!loadedScenes.ContainsKey(sceneName))
            throw new ArgumentException($"Can't exit scene \"{sceneName}\". use Scene.Enter(sceneName) to load your scene.");

        // unload scene
        if (unload)
        {
            Unload(sceneName);
        }
        // set scene inactive
        else
        {
            var rootObjects = loadedScenes[sceneName].GetRootGameObjects();

            foreach (var obj in rootObjects)
            {
                activeObjects[sceneName][obj.GetInstanceID()] = obj.activeSelf;

                // dont disable timers!!
                if (obj.TryGetComponent(out Timer _))
                    continue;

                obj.SetActive(false);
            }
        }

        // set main scene active again
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(StringLiterals.MAIN_SCENE));

        foreach (var obj in SceneManager.GetSceneByName(StringLiterals.MAIN_SCENE).GetRootGameObjects())
            if (activeObjects[StringLiterals.MAIN_SCENE].ContainsKey(obj.GetInstanceID()))
                obj.SetActive(activeObjects[StringLiterals.MAIN_SCENE][obj.GetInstanceID()]);

        loading = false;
        OnExit?.Invoke();
    }

    public static void Exit(bool unload = false) => Exit(SceneManager.GetActiveScene().name, unload);

    public static void ClearAll()
    {
        foreach(var loadedScene in loadedScenes.Keys)
        {
            if (loadedScene == StringLiterals.MAIN_SCENE)
                continue;

            Unload(loadedScene);
        }
    }

    public static async void Unload(string sceneName)
    {
        await SceneManager.UnloadSceneAsync(sceneName);
        loadedScenes.Remove(sceneName);
        activeObjects.Remove(sceneName);
    }
}
