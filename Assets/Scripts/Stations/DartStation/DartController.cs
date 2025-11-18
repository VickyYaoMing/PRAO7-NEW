using System;
using System.Collections;
using UnityEngine;

public class DartController : TaskBase
{
    [SerializeField] private GameObject arrowObject;
    private int maximumAmountOfTries = 3;
    private GameObject[] arrows;
    private int currentArrowIndex = 0;
    private Vector3 defaultPosition;
    private Quaternion defaultRotation;

    void Awake()
    {
        arrows = new GameObject[maximumAmountOfTries];
        for(int i = 0; i < maximumAmountOfTries; i++)
        {
            arrows[i] = Instantiate(arrowObject);
            arrows[i].SetActive(false);
        }

        arrows[currentArrowIndex].SetActive(true);
        defaultPosition = arrows[currentArrowIndex].transform.position;
        defaultRotation = arrows[currentArrowIndex].transform.rotation;
        currentArrowIndex++;
    }

    private void OnReset()
    {
        MissionWasAccomplished = false;
        for(int i = 0; i < arrows.Length; i++)
        {
            arrows[i].SetActive(false);
            arrows[i].transform.position = defaultPosition;
            arrows[i].transform.rotation = defaultRotation;
        }
        currentArrowIndex = 0;
        arrows[currentArrowIndex].SetActive(true);
        currentArrowIndex++;
    }
    private void OnEnable()
    {
        OnReset();
        DartLogic.OnDartHit += OnHit;
        DartLogic.OnDartMissed += OnMiss;
    }

    private void OnDisable()
    {
        DartLogic.OnDartHit -= OnHit;
       DartLogic.OnDartMissed -= OnMiss;

    }
    private void OnHit()
    {
        MissionWasAccomplished = true;
        Exit(true);
    }
    private void OnMiss()
    {
        StartCoroutine(ActivateNewArrow());
    }

    private IEnumerator ActivateNewArrow()
    {
        yield return new WaitForSeconds(1f);

        if (currentArrowIndex >= maximumAmountOfTries) 
        {
            OnReset();
        }
        else
        {
            arrows[currentArrowIndex].SetActive(true);
            currentArrowIndex++;
        }
    }
    protected override void Update()
    {
        base.Update();
        
    }
}
