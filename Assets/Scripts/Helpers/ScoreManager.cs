using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    private static int totalPoints = 0;

    private void OnEnable()
    {
        TaskGiver.taskScore += CalculateTotalScore;
    }
    private void OnDisable()
    {
        TaskGiver.taskScore -= CalculateTotalScore;
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void CalculateTotalScore(int points)
    {
        points += totalPoints;
    }

}
