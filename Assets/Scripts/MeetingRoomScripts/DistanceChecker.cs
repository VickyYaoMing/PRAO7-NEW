using UnityEngine;
using System.Collections.Generic;

public class SimpleObjectSpawner : MonoBehaviour
{
    [Header("Spawning Settings")]
    public GameObject objectPrefab;
    public KeyCode spawnKey = KeyCode.E;

    [Header("Spawn Location")]
    public Transform spawnLocation; 

    private List<GameObject> spawnedObjects = new List<GameObject>();

    private void Update()
    {
        if (Input.GetKeyDown(spawnKey))
        {
            SpawnObject();
        }

        if (Input.GetKeyDown(KeyCode.Q) && spawnedObjects.Count > 0)
        {
            DeleteLastObject();
        }
    }

    private void SpawnObject()
    {
        if (objectPrefab == null)
        {
            Debug.LogWarning("No object prefab assigned!");
            return;
        }

        if (spawnLocation == null)
        {
            Debug.LogWarning("No spawn location assigned!");
            return;
        }

        GameObject newObject = Instantiate(objectPrefab, spawnLocation.position, spawnLocation.rotation);

        newObject.name = $"{objectPrefab.name}_{spawnedObjects.Count:00}";

        // Add to spawned objects list
        spawnedObjects.Add(newObject);

        Debug.Log($"Spawned object at {spawnLocation.position}. Total objects: {spawnedObjects.Count}");
    }

    private void DeleteLastObject()
    {
        if (spawnedObjects.Count > 0)
        {
            GameObject lastObject = spawnedObjects[spawnedObjects.Count - 1];
            spawnedObjects.Remove(lastObject);
            Destroy(lastObject);
            Debug.Log($"Deleted last object. Total objects: {spawnedObjects.Count}");
        }
    }

    public void ClearAllObjects()
    {
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null)
                Destroy(obj);
        }
        spawnedObjects.Clear();
        Debug.Log("Cleared all objects");
    }

#if UNITY_EDITOR
    [ContextMenu("Spawn Object Now")]
    private void EditorSpawnObject()
    {
        if (Application.isPlaying)
        {
            SpawnObject();
        }
        else
        {
            Debug.Log("Can only spawn objects during play mode");
        }
    }

    [ContextMenu("Clear All Objects")]
    private void EditorClearAllObjects()
    {
        if (Application.isPlaying)
        {
            ClearAllObjects();
        }
    }

    [ContextMenu("Set Spawn Location to Selected")]
    private void SetSpawnLocationToSelected()
    {
        if (UnityEditor.Selection.activeTransform != null)
        {
            spawnLocation = UnityEditor.Selection.activeTransform;
            UnityEditor.EditorUtility.SetDirty(this);
            Debug.Log($"Spawn location set to: {spawnLocation.name}");
        }
    }
#endif
}