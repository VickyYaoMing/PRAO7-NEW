using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;


public class WhiteboardUi : MonoBehaviour
{

    [Header("Score Display")]
    [SerializeField] public TMP_Text scoreText;

    [Header("Scene Change")]
    private Dictionary<string, string> nextSceneMap = new Dictionary<string, string>
    {
        {StringLiterals.DAY1_SCENE, StringLiterals.DAY2_SCENE },
        {StringLiterals.DAY2_SCENE, StringLiterals.DAY3_SCENE },
        {StringLiterals.DAY3_SCENE, StringLiterals.STARTMENU_SCENE },
    };

    [Header("Table Cells")]
    [Header("Row 1")]
    [SerializeField] public TMP_Text row1Col1;
    [SerializeField] public TMP_Text row1Col2;
    [SerializeField] public TMP_Text row1Col3;
    [SerializeField] public TMP_Text row1Col4;

    [Header("Row 2")]
    [SerializeField] public TMP_Text row2Col1;
    [SerializeField] public TMP_Text row2Col2;
    [SerializeField] public TMP_Text row2Col3;
    [SerializeField] public TMP_Text row2Col4;

    [Header("Row 3")]
    [SerializeField] public TMP_Text row3Col1;
    [SerializeField] public TMP_Text row3Col2;
    [SerializeField] public TMP_Text row3Col3;
    [SerializeField] public TMP_Text row3Col4;

    [Header("Row 4")]
    [SerializeField] public TMP_Text row4Col1;
    [SerializeField] public TMP_Text row4Col2;
    [SerializeField] public TMP_Text row4Col3;
    [SerializeField] public TMP_Text row4Col4;

    private void Start()
    {
        UpdateScore(ScoreManager.totalPoints);


        // temp placeholder data, should be called like this elsewhere? idk how else to do this lol 
        string[] placeholderNames = { "Printing Task", "Coffee Task", "Cleanup Task", "Dartboard Task" };
        //float[] placeholderTime = { 10.4f, 48.23f, 130.32f, 44.23f };
        //int[] placeholderClicks = { 10, 45, 43, 23 };
        //int[] placeholderTimesPlayer = { 2309, 12319, 2393, 22 };


        float[] placeholderTime = { 
            TestingAnalytics.Instance.CalculateAverageTimeInSeconds(taskEnum.Printer),
            TestingAnalytics.Instance.CalculateAverageTimeInSeconds(taskEnum.Coffee),
            TestingAnalytics.Instance.CalculateAverageTimeInSeconds(taskEnum.Mopping),
            TestingAnalytics.Instance.CalculateAverageTimeInSeconds(taskEnum.Dart) };

        float[] placeholderClicks = { TestingAnalytics.Instance.CalculateAverageClicks(taskEnum.Printer),
            TestingAnalytics.Instance.CalculateAverageClicks(taskEnum.Coffee),
            TestingAnalytics.Instance.CalculateAverageClicks(taskEnum.Mopping),
            TestingAnalytics.Instance.CalculateAverageClicks(taskEnum.Dart) };

        int[] placeholderTimesPlayer = { TestingAnalytics.Instance.ReturnTimesPlayed(taskEnum.Printer),
            TestingAnalytics.Instance.ReturnTimesPlayed(taskEnum.Coffee),
            TestingAnalytics.Instance.ReturnTimesPlayed(taskEnum.Mopping),
            TestingAnalytics.Instance.ReturnTimesPlayed(taskEnum.Dart)};

        UpdateTable(placeholderNames, placeholderTime, placeholderClicks, placeholderTimesPlayer);

    }

    public void UpdateScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }


    public void UpdateTable(string[] names, float[] avgTime, float[] avgClicks, int[] timesPlayed)
    {
        row1Col1.text = names[0];
        row1Col2.text = avgTime[0].ToString();
        row1Col3.text = avgClicks[0].ToString();
        row1Col4.text = timesPlayed[0].ToString();

        row2Col1.text = names[1];
        row2Col2.text = avgTime[1].ToString();
        row2Col3.text = avgClicks[1].ToString();
        row2Col4.text = timesPlayed[1].ToString();

        row3Col1.text = names[2];
        row3Col2.text = avgTime[2].ToString();
        row3Col3.text = avgClicks[2].ToString();
        row3Col4.text = timesPlayed[2].ToString();

        row4Col1.text = names[3];
        row4Col2.text = avgTime[3].ToString();
        row4Col3.text = avgClicks[3].ToString();
        row4Col4.text = timesPlayed[3].ToString();
    }

    // Change Scene, this is written with the assumption that end game screen does not become main scene
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
            writer.WriteLine("Minigame,Avg Time,Avg Clicks,Times Played");

            WriteRow(writer, "Printer", taskEnum.Printer);
            WriteRow(writer, "Coffee", taskEnum.Coffee);
            WriteRow(writer, "Mopping", taskEnum.Mopping);
            WriteRow(writer, "Dart", taskEnum.Dart);
        }

        Debug.Log("task_stats.csv exported to: " + path);
    }

    private void WriteRow(StreamWriter writer, string label, taskEnum task)
    {
        float avgTime = TestingAnalytics.Instance.CalculateAverageTimeInSeconds(task);
        float avgClicks = TestingAnalytics.Instance.CalculateAverageClicks(task);
        int timesPlayed = TestingAnalytics.Instance.ReturnTimesPlayed(task);

        writer.WriteLine($"{label}, {avgTime}, {avgClicks}, {timesPlayed}");
    }
}