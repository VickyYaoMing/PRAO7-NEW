using UnityEngine;

public class CurrentBaseDataCollector : MonoBehaviour
{
    taskEnum taskID; // task ID can be turned into their days?
    int mouseClicks = 0;
    bool MissionWasAccomplished;
    int testNumber;// can be if we win or lose in their game 


    // Method below should run when u click to export the data;
    public void collectData()
    {
       TestingAnalytics.Instance.RecordMinigameData(taskID, Time.timeSinceLevelLoad, mouseClicks, MissionWasAccomplished);

    }

// Update is called once per frame
void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseClicks++;
            ScreenCapture.CaptureScreenshot("screenshots" + testNumber);
            testNumber++;
        }

      // Need something to reset the mouseclick after ....a point? 
      // they have an enum for which day it is, if we get that information we could use that. 
      // we gotta check their code -> see what we can use;
    }
}
