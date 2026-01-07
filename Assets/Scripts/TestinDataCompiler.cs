using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestinDataCompiler: MonoBehaviour
{
    public void OnButtonPress()
    {
        Scene.ResetStatics();

        if (StringLiterals.MAIN_SCENE == StringLiterals.DAY1_SCENE)
        {
            StringLiterals.MAIN_SCENE = StringLiterals.DAY2_SCENE;
            SceneManager.LoadScene(StringLiterals.MAIN_SCENE);
            return;
        }
        else if (StringLiterals.MAIN_SCENE == StringLiterals.DAY2_SCENE)
        {
            StringLiterals.MAIN_SCENE = StringLiterals.DAY3_SCENE;
            SceneManager.LoadScene(StringLiterals.MAIN_SCENE);
            return;
        }
        else if (StringLiterals.MAIN_SCENE == StringLiterals.DAY3_SCENE)
        {
            StringLiterals.MAIN_SCENE = StringLiterals.DAY1_SCENE;
            SceneManager.LoadScene(StringLiterals.STARTMENU_SCENE);
            return;
        }
    }
    public void ExportStatsToCSV()
    {
        string timeStamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string path = Path.Combine(Application.persistentDataPath, $"task_stats{timeStamp}.csv");

        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.WriteLine("Day: " + StringLiterals.MAIN_SCENE.ToString().Replace("Day", string.Empty));
            writer.WriteLine("Minigame, Total Time, Total Clicks, Mission Accomplished");

            for (int i = 0; i < TestingAnalytics.Instance.allLists.Count; i++)
            {
                for (int j = 0; j < TestingAnalytics.Instance.allLists[i].Count; j++)
                {
                    var row = TestingAnalytics.Instance.allLists[i][j];
                    writer.WriteLine($"{row.taskEnum + " " + j}, {row.time}, {row.clicks}, {row.accomplished}");
                }
            }
        }

        Debug.Log("task_stats.csv exported to: " + path);
        TestingAnalytics.Instance.ResetTesting();
    }




}
