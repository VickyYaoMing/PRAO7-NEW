using UnityEngine;

public class MainMenuCamera : MonoBehaviour
{
    [SerializeField]Camera cam;
    Vector3 startPosition;
    [SerializeField]GameObject emptyEndPos;
    Vector3 endPos;

    // apparently I can get camera as a inherited object but idk how to do that so it is serialized lol
    // we need the empty end pos because that is the position that is going to be lerped too, makes it so that if we do scale the elevator, it will not completely screw over everything.

    void Start()
    {
        if (cam != null)
        {
            startPosition = cam.GetComponent<Transform>().position;
            endPos = emptyEndPos.transform.position;
            cam.fieldOfView = 60;
        }
    }

    void Update()
    {
        startPosition = cam.GetComponent<Transform>().position;
        cam.GetComponent<Transform>().position = Vector3.Lerp(startPosition, endPos, 0.01f);

    }
}
