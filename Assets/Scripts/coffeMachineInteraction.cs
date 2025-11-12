using System;
using UnityEditorInternal;
using UnityEngine;
using System.Collections.Generic;

public class coffeMachineInteraction : MonoBehaviour
{
    public static float brewTime = 20f;

    public Collider startButtonCollider;
    public Collider leverCollider;
    public Collider coffeTriggerCollider;
    public bool m_brewing = false;
    public GameObject coffe;
    bool m_onButtonPressed;

    public bool brewedDone = false;

    private void Start()
    {
        coffe.SetActive(false);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public void StartMachine(RaycastHit rayHit)
    {
        if (rayHit.collider == startButtonCollider && Input.GetMouseButtonDown(0))
        {
            m_onButtonPressed = true;
            // play start sound
            Debug.Log("machine on");
        }
    }

    public void BrewCoffe(RaycastHit rayHit)
    {
        if (m_onButtonPressed)
        {
            if (rayHit.collider == leverCollider && Input.GetMouseButtonDown(0))
            {
                coffe.SetActive(true);
                Timer.Create(brewTime, true, true, true, "CoffeTimer").Timeout += BrewDone;
            }
        }
    }

    public void BrewDone()
    {
        brewedDone = true;
        coffe.SetActive(false);
    }


}

