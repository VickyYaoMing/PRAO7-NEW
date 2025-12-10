using System;
using Unity.VisualScripting;
using UnityEditor.Rendering;
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
    float time;
    public static int clicks;
    public bool locked;
  

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
        clicks = 0; // questionmarl on this one aswell
        time = 0;
    }

    public void Done()
    {
        fullCoffeCup.SetActive(true);
        fullCoffeCup.transform.position = transform.position;
        transform.position = transform.position + new Vector3(-0.55f, 10, 0);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        Vector3 m_mousePosition = Input.mousePosition;

        if (( Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(1)))
        {
            clicks += 1;
        } // should this be here? idkkkk helppppppppppppp


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
                        int finalClicks = clicks - 1; // since the coffecup is bein replaced, it counts as clicing both of them for whatev rason
                        fullCoffeCup.SetActive(false);
                        taskID = taskEnum.Coffee;
                        time = Time.realtimeSinceStartup; // als don know if this should be here
                        InteractionManager.Instance.OnRecieveItem(taskID);
                        TestingAnalytics.Instance.LogIfItsBeenPlayed(taskID);
                        TestingAnalytics.Instance.LogHowLongBeenPlayed(taskID, time);
                        TestingAnalytics.Instance.LogHowManyClicks(taskID, clicks);
                        //Debug.Log("time" + time);   logs used to debug to make sure the correct data gr sent
                        //Debug.Log("clicks" + finalClicks); log used ot ensure correct data ws sent in method
                        Exit();
                    }
                    if(!locked)
                    {
                        m_held = true;
                        m_DistanceToCamera = Vector3.Distance(m_Camera.transform.position, transform.position);
                    }
                  
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
