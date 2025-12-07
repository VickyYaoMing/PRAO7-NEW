using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.WSA;

public class DartLogic : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidBody;


    private float maxSideOffset = 0.2f;    
    private float aimSensitivity = 0.0005f; 

    private float maxAimAngle = 30f;
    private Vector3 restPosition;
    private Quaternion restRotation;
    private Vector3 mouseStartPosition = Vector3.zero;
    private float currentPull;
    private float maxPullDistance = 0.2f;    
    private float maxLaunchSpeed = 22f;

    public static Action OnDartMissed;
    public static Action OnDartHit;
    private bool hasCollidedOnce = false;
    private bool hasBeenShot = false;
    private bool arrowHasBeenClicked = false;


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
        if (hasBeenShot && transform.position.y < 0 && !hasCollidedOnce)
        {
            OnDartMissed?.Invoke();
            hasCollidedOnce = true;
        }

    }

    private void OnEnable()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        hasCollidedOnce = false;
    }
    private void OnCollisionEnter(Collision other)
    {
        if (hasCollidedOnce) return;

            rigidBody.isKinematic = true;
            OnDartMissed?.Invoke();
            hasCollidedOnce = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DartBoard"))
        {
            rigidBody.isKinematic = true;
            OnDartHit?.Invoke();
            hasCollidedOnce = true;
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

            float pullRaw = -mouseDelta.y * aimSensitivity;  
            currentPull = Mathf.Clamp(pullRaw, 0f, maxPullDistance);

            float sideOffset = Mathf.Clamp(mouseDelta.x * aimSensitivity, -maxSideOffset, maxSideOffset);
            Vector3 newPos = restPosition + transform.right * sideOffset - transform.forward * currentPull;
            transform.position = newPos;

            float currentPitch = Mathf.Clamp(-mouseDelta.y * 0.01f, -maxAimAngle, maxAimAngle);
            float currentYaw = Mathf.Clamp(mouseDelta.x * 0.01f, -maxAimAngle, maxAimAngle);

            Quaternion aimRotation = Quaternion.Euler(-currentPitch, -currentYaw, 0f);
            transform.rotation = restRotation * aimRotation;
        }
    }


    private void OnArrowRelease()
    {
        if (Input.GetMouseButtonUp(0) && arrowHasBeenClicked)
        {
            hasBeenShot = true;
            arrowHasBeenClicked = false;

            float pull01 = currentPull / maxPullDistance;
            float launchSpeed = pull01 * maxLaunchSpeed;

            rigidBody.isKinematic = false;
            rigidBody.useGravity = true;

            Vector3 launchDir = transform.forward;

            rigidBody.linearVelocity = launchDir.normalized * launchSpeed;
            currentPull = 0f;
        }
    }




}
