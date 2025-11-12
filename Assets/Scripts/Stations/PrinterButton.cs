using UnityEngine;

public class PrinterButton : MonoBehaviour
{
    [Header("Button Visuals")]
    public Renderer buttonRenderer;
    public Color hoverColor = Color.white;

    private Color originalColor;
    private PrinterController printerController;
    private bool isActive = true;
    private Material buttonMaterial;
    public Camera mainCamera;

    void Start()
    {

        //check for collider
        if (GetComponent<Collider>() == null)
            gameObject.AddComponent<BoxCollider>();
    }

    public void Initialize(Color color, PrinterController controller)
    {
        printerController = controller;

        if (buttonRenderer == null)
            buttonRenderer = GetComponent<Renderer>();

        if (buttonRenderer != null)
        {
            buttonMaterial = buttonRenderer.material;
            buttonMaterial.color = color;
            originalColor = color;
        }
    }

    void Update()
    {
        if (!isActive) return;

        //raycast detection
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    OnButtonClicked();
                }
            }
        }

        // Hover detection
        Ray hoverRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hoverHit;

        if (Physics.Raycast(hoverRay, out hoverHit))
        {
            if (hoverHit.collider.gameObject == this.gameObject)
            {
                OnMouseHoverEnter();
            }
            else
            {
                OnMouseHoverExit();
            }
        }
        else
        {
            OnMouseHoverExit();
        }
    }

    void OnButtonClicked()
    {
        Debug.Log("Button clicked: " + originalColor + " - " + gameObject.name);
        printerController.ButtonPressed(originalColor);
    }

    void OnMouseHoverEnter()
    {
        if (isActive && buttonMaterial != null)
        {
            buttonMaterial.color = hoverColor;
        }
    }

    void OnMouseHoverExit()
    {
        if (isActive && buttonMaterial != null)
        {
            buttonMaterial.color = originalColor;
        }
    }

    public void SetActive(bool active)
    {
        isActive = active;

        //enable/disable collider
        Collider collider = GetComponent<Collider>();
        if (collider != null)
            collider.enabled = active;

        if (buttonMaterial != null)
        {
            buttonMaterial.color = active ? originalColor : Color.gray;
        }
    }
}