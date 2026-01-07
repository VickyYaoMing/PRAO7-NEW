using UnityEngine;

public class MouseCourser : MonoBehaviour
{
    [SerializeField]private Camera main;
    public LayerMask floorLayer;
    public float heigthOffSet = 1.5f;

    // Update is called once per frame
    void Update()
    {
        Ray ray = main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, floorLayer))
        {
            Vector3 targetPos = hit.point;
            targetPos.y += heigthOffSet; // Optional offset so it floats above floor
            transform.position = targetPos;
        }
    }
}

