using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class TrashScript : ClickableObjects
{
    bool isDragging = false;
    Vector3 dragOffset;
    Vector3 messyPos;
    Vector3 mousePos;
    float objectDepth;//distance from camera to object
    private Camera cam;
    [SerializeField] GameObject bintrash;
    void Start()
    {
        messyPos = gameObject.transform.position;
        
        cam = Camera.main;
        ApplyState();
    }
    public override void ApplyState()
    {
        if (isCleaned)
        {
            gameObject.SetActive(false);

        }
        else
        {
            gameObject.SetActive(true);
            //reset the pos so that the trash moves back
            gameObject.transform.position = messyPos;

        }
    }
    protected override void OnClicked()
    {
        //check if its not cleand and off mouse is still down, if its down the trash pos is the mouse pos
        //when mosue click is realeades if the trash collied with the trashbins collied then the trash gets set to non active
        //if not then the trashpos is reset to the messy pos it had in the begining aka call the apply state? add in messy pos to the script vector 3

        //if (!isCleaned)
        //{
        //    gameObject.SetActive(false);
        //    amountCleaned++;
        //    UpdateAboutCleaning?.Invoke(amountCleaned);
        //    isCleaned = true;
        //}
        if (!isCleaned)
        {
            isDragging=true;
            //get current mouse pos
            objectDepth = Vector3.Distance(cam.transform.position, transform.position);
            
            mousePos = Input.mousePosition;
            mousePos.z = objectDepth;

            Vector3 worldMouse = cam.ScreenToWorldPoint(mousePos);

            dragOffset = transform.position - worldMouse;

            
        }

    }
    public void OnMouseDrag()
    {
        if (!isDragging) return;


        mousePos = Input.mousePosition;
        mousePos.z = objectDepth;

        Vector3 worldMouse = cam.ScreenToWorldPoint(mousePos);
        
        transform.position = worldMouse+dragOffset;
    }
    /// <summary>
    /// once the mouse is not clicked and the bool is false 
    /// check if the mosue is colliding with the trashBin
    /// if true then setActive(False);
    /// call fillTrashbin() for visual representation of cleaning
    /// if not reset position to begining
    /// </summary>
    public void OnMouseUp()
    {
       if (!isDragging) return;

        isDragging=false;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit))
        {
            if (hit.collider.CompareTag("TrashBin"))
            {
                gameObject.SetActive(false);
                isCleaned=true;
                //Debug.Log("item cleaned" + gameObject.name);
                amountCleaned++;
                FillTrasBin();

                UpdateAboutCleaning?.Invoke(amountCleaned);
                return;

            }
        }
        ApplyState();
    }
    /// <summary>
    /// Initializes the vector of the trashbin
    /// moves up the y position of the trashobject
    /// </summary>
   void FillTrasBin()
    {
       
        Vector3 binTrashPos= bintrash.transform.position;

        binTrashPos.y += 0.05f;

        bintrash.transform.position= binTrashPos;
        
    }


}


