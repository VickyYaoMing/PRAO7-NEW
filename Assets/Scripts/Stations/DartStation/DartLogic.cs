using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.WSA;

public class DartLogic : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidBody;
    private float maxAimAngle = 30f;      
    private float aimSensitivity = 0.1f;
    private bool arrowHasBeenClicked = false;
    private Vector3 restPosition;
    private Quaternion restRotation;
    private Vector3 mouseStartPosition = Vector3.zero;
    private float currentPull;
    private float currentAimAngle;
    private float gravity = -9.82f;

    private float maxPullDistance = 50f;    
    private float maxLaunchSpeed = 1500f;
    void Start()
    {
        restPosition = transform.position;
        restRotation = transform.rotation;
        rigidBody.isKinematic = true;
    }

    void Update()
    {
        OnArrowClick();
        OnArrowHold();
        OnArrowRelease();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DartBoard"))
        {
            rigidBody.isKinematic = true;
        }
        else if (other.gameObject.CompareTag("Wall"))
        {
            rigidBody.isKinematic = true;

        }
    }

    private void OnArrowClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000f))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    arrowHasBeenClicked = true;
                    mouseStartPosition = Input.mousePosition;
                    restPosition = transform.position;
                    restRotation = transform.rotation;

                    rigidBody.isKinematic = true;
                }
            }
        }
    }

    private void OnArrowHold()
    {
        if (Input.GetMouseButton(0) && arrowHasBeenClicked)
        {
            Vector3 mouseDelta = Input.mousePosition - mouseStartPosition;

            float dragAmount = mouseDelta.magnitude * 0.1f;
            currentPull = Mathf.Clamp(dragAmount, 0f, maxPullDistance);
            transform.position = restPosition - transform.forward * currentPull;
            //transform.position = new Vector3()

            currentAimAngle = Mathf.Clamp(mouseDelta.y * aimSensitivity, -maxAimAngle, maxAimAngle);

            Quaternion pitch = Quaternion.AngleAxis(-currentAimAngle, transform.right);
            transform.rotation = pitch * restRotation;
        }
    }


    private void OnArrowRelease()
    {
        if (Input.GetMouseButtonUp(0) && arrowHasBeenClicked)
        {
            arrowHasBeenClicked = false;

            float pull01 = currentPull / maxPullDistance;
            float launchSpeed = pull01 * maxLaunchSpeed;

            rigidBody.isKinematic = false;

            Vector3 horiz = new Vector3(transform.forward.x, 0f, transform.forward.z).normalized;

            float angleRad = currentAimAngle * Mathf.Deg2Rad;

            float vxz = launchSpeed * Mathf.Cos(angleRad);
            float vy = launchSpeed * Mathf.Sin(angleRad); 

            Vector3 launchVelocity = horiz * vxz + Vector3.up * vy;

            rigidBody.linearVelocity = launchVelocity;

            currentPull = 0f;
        }
    }



}
