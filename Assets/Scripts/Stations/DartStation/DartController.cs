using System;
using System.Collections;
using UnityEngine;

public class DartController : TaskBase
{
    [SerializeField] private GameObject arrowObject;
    private int maximumAmountOfTries = 10;
    private GameObject[] arrows;
    private int currentArrowIndex = 0;
    void Start()
    {
        arrows = new GameObject[maximumAmountOfTries];
        for(int i = 0; i < maximumAmountOfTries; i++)
        {
            arrows[i] = Instantiate(arrowObject);
            arrows[i].SetActive(false);
        }

        arrows[currentArrowIndex].SetActive(true);
        currentArrowIndex++;
    }

    private void OnReset()
    {
        for(int i = 0; i < maximumAmountOfTries; i++)
        {
            arrows[i].SetActive(false);
        }
        currentArrowIndex = 0;
        arrows[currentArrowIndex].SetActive(true);
        currentArrowIndex++;
    }
    private void OnEnable()
    {
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
        StartCoroutine(TimerForExiting());
    }
    private IEnumerator TimerForExiting()
    {
        yield return new WaitForSeconds(3f);
        Exit();
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
            Debug.LogWarning("You suck no more tries for you, come back later we will need to buy in more arrows");
            yield return new WaitForSeconds(0f);
        }

        arrows[currentArrowIndex].SetActive(true);
        currentArrowIndex++;

    }
    protected override void Update()
    {
        base.Update();
        
    }
}
