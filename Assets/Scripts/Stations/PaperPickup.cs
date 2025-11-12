using UnityEngine;

public class PaperPickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    public float pickupDistance = 3f;
    public float holdDistance = 2f;
    public float movementSmoothness = 10f;
    public float rotationSmoothness = 10f;
    public bool lockCursor = true;
    [Header("Physics Settings")]
    public float throwForce = 10f;
    public LayerMask pickupLayerMask = -1;

    private Rigidbody paperRigidbody;
    private bool isHolding = false;
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private Vector3 previousMousePosition;
    private float currentHoldDistance;
    public Camera playerCamera;
    void Start()
    {
        paperRigidbody = GetComponent<Rigidbody>();
        currentHoldDistance = holdDistance; // Initialize with the set hold distance

        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        //Debug.Log("Initial hold distance: " + currentHoldDistance);
    }

    void Update()
    {
        HandleInput();

        if (isHolding)
        {
            UpdateHoldPosition();
            HandleScrollInput();
        }
    }

    void HandleInput()
    {
        // Start holding on left mouse button down
        if (Input.GetMouseButtonDown(0) && !isHolding)
        {
            TryPickup();
        }

        // Release on left mouse button up
        if (Input.GetMouseButtonUp(0) && isHolding)
        {
            Release();
        }

        // Throw on right mouse button while holding
        if (Input.GetMouseButtonDown(1) && isHolding)
        {
            Throw();
        }
    }

    void TryPickup()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickupDistance))
        {
            if (hit.collider.gameObject == gameObject)
            {
                StartHolding();
            }
        }
    }

    void StartHolding()
    {
        isHolding = true;

        //Disable physics while holding
        paperRigidbody.isKinematic = true;
        paperRigidbody.useGravity = false;
        //decide hold distance based on camera
        Vector3 cameraToPaper = transform.position - playerCamera.transform.position; 
        float currentDistance = cameraToPaper.magnitude;

        //Use either the current distance or the preset hold distance, whichever is closer
        currentHoldDistance = Mathf.Clamp(currentDistance, 0.5f, pickupDistance);

        Debug.Log("Started holding. Current distance: " + currentDistance + ", Hold distance: " + currentHoldDistance);
    }

    void UpdateHoldPosition()
    {
        //Get camera position and forward direction
        Vector3 cameraPosition = playerCamera.transform.position;
        Vector3 cameraForward = playerCamera.transform.forward;

        //Calculate target position using the current hold distance
        Vector3 targetPosition = cameraPosition + (cameraForward * currentHoldDistance);

        //Debug.Log("Camera position: " + cameraPosition +
        //         " | Target position: " + targetPosition +
        //         " | Hold distance: " + currentHoldDistance);

        transform.position = Vector3.Lerp(transform.position, targetPosition, movementSmoothness * Time.deltaTime);

        // Make paper face camera but stay upright
        Vector3 directionToCamera = playerCamera.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(-directionToCamera, Vector3.up);

        Vector3 euler = targetRotation.eulerAngles;
        euler.z = 0; // Remove roll
        euler.x = 0; // Remove pitch (keep flat)
        targetRotation = Quaternion.Euler(euler);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSmoothness * Time.deltaTime);
    }

    void HandleScrollInput()
    {
        // Adjust hold distance with scroll wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            currentHoldDistance -= scroll * 2f;
            currentHoldDistance = Mathf.Clamp(currentHoldDistance, 0.5f, pickupDistance);

            Debug.Log("Scroll input: " + scroll + " | New hold distance: " + currentHoldDistance);
        }

        if (Input.GetKey(KeyCode.Equals) || Input.GetKey(KeyCode.Plus))
        {
            currentHoldDistance += 1f * Time.deltaTime;
            currentHoldDistance = Mathf.Clamp(currentHoldDistance, 0.5f, pickupDistance);
        }
        if (Input.GetKey(KeyCode.Minus))
        {
            currentHoldDistance -= 1f * Time.deltaTime;
            currentHoldDistance = Mathf.Clamp(currentHoldDistance, 0.5f, pickupDistance);
        }
    }

    void Release()
    {
        if (!isHolding) return;

        isHolding = false;

        //Re-enable physics
        paperRigidbody.isKinematic = false;
        paperRigidbody.useGravity = true;

        //Debug.Log("Paper released");
    }

    void Throw()
    {
        if (!isHolding) return;

        isHolding = false;

        //Re-enable physics
        paperRigidbody.isKinematic = false;
        paperRigidbody.useGravity = true;

        //Apply throw force in camera direction
        Vector3 throwDirection = playerCamera.transform.forward;
        paperRigidbody.AddForce(throwDirection * throwForce, ForceMode.Impulse);
    }

    void OnDrawGizmos()
    {
        if (playerCamera != null && isHolding)
        {
            // Draw line from camera to paper
            Gizmos.color = Color.green;
            Gizmos.DrawLine(playerCamera.transform.position, transform.position);

            // Draw sphere at target position
            Gizmos.color = Color.red;
            Vector3 targetPos = playerCamera.transform.position + playerCamera.transform.forward * currentHoldDistance;
            Gizmos.DrawWireSphere(targetPos, 0.1f);

            // Draw camera forward direction
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * currentHoldDistance);
        }
    }
}