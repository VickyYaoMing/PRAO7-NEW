using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class coffeCup_logic : TaskBase
{
    public Camera m_Camera;
    RaycastHit m_raycastHit;
    float m_DistanceToCamera;
    bool m_held = false;
    public Rigidbody m_Rigidbody;
    public GameObject fullCoffeCup;
    GameObject coffeCup;
  

    [SerializeField]public bool m_inPosition = false;
    [SerializeField] public bool done;
    Vector3 maxVelocity;
    Vector3 test;
    public bool IsHeld
    {
        get { return m_held; }
        set { m_held = value; }
    }
    public Transform CupTransform
    {
        get { return transform; }
        set {  value.parent = transform; }
    }
    Vector3 offset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxVelocity = new Vector3(1, 1, 0.4f);
        fullCoffeCup.SetActive(false);
    }

    public void Done()
    {
        fullCoffeCup.SetActive(true);
        fullCoffeCup.transform.position = transform.position;
        transform.position = transform.position + new Vector3(-0.55f, 10, 0);
        done = true;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        Vector3 m_mousePosition = Input.mousePosition;

        Ray m_ray = m_Camera.ScreenPointToRay(m_mousePosition);

        bool m_hitSmtgh = Physics.Raycast(m_ray, out m_raycastHit);
        if (m_hitSmtgh)
        {
           

            if (m_raycastHit.transform == transform  && !m_held)
            {
                if (Input.GetMouseButton(0))
                {
                    if (done)
                    {
                        done = false;
                        fullCoffeCup.SetActive(false);
                        taskID = taskEnum.Coffee;
                        InteractionManager.Instance.OnRecieveItem(taskID);
                        Exit();
                    }
                    m_held = true;
                    m_DistanceToCamera = Vector3.Distance(m_Camera.transform.position, transform.position);
                }           
            }
        }
        if (m_held)
        {
            m_Rigidbody.rotation = Quaternion.identity;
            m_Rigidbody.useGravity = false;
            Vector3 worldPoint = m_Camera.ScreenToWorldPoint(new Vector3(m_mousePosition.x, m_mousePosition.y,1.7f));
            offset = new Vector3 ( worldPoint.x , worldPoint.y-0.1f, worldPoint.z );
            m_Rigidbody.MovePosition(offset);
            if (Input.GetMouseButton(1))
            { m_held = false;
               m_Rigidbody.useGravity = true;
            }

        }

    }
}
