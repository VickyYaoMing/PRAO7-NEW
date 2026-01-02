using Mono.Cecil.Cil;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static Unity.Burst.Intrinsics.X86.Avx;

public class cupManager : MonoBehaviour
{
    int time = 0;
    int sec;


    public List<coffeCup_logic> allCups = new List<coffeCup_logic>();
    public coffeMachineInteraction m_coffeMachine;
    public Camera m_camera;
    Collider cupCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        allCups.AddRange(FindObjectsByType<coffeCup_logic>(FindObjectsSortMode.None));
    }

    // Update is called once per frame
    void Update()
    {
        if(m_coffeMachine.m_brewing)
        {        
            sec = 1;
            time += sec;
            Debug.Log("timer" + time);
        }
        Ray m_ray = m_camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit m_raycastHit;
        bool m_hitSmtgh = Physics.Raycast(m_ray, out m_raycastHit);
        if (m_hitSmtgh)
        {
            m_coffeMachine.StartMachine(m_raycastHit);
            foreach (var cup in allCups)
            {
                if (cup.m_inPosition)
                {
                  //  cup.locked = true;
                    m_coffeMachine.BrewCoffe(m_raycastHit);
               
                }             
            }
        }

        foreach (var cup in allCups)
        {
            if (m_coffeMachine.brewedDone)
            {
                cup.locked = false;
                cup.Done();
                m_coffeMachine.coffe.SetActive(false);
                m_coffeMachine.brewedDone = false;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TESTING");
        foreach (var cup in allCups)
        {
           cupCollider = cup.GetComponent<Collider>();
          if ( other == cupCollider)
            {
                cup.m_inPosition = true;
                Debug.Log("ready");
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        foreach (var cup in allCups)
        {
            cupCollider = cup.GetComponent<Collider>();
            if (other == cupCollider)
            {
                cup.m_inPosition = false;
                cup.enabled = true;
            }
        }
    }

}


