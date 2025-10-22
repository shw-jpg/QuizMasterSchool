using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    int currentScore = 0;
    int questionSeen = 0;

    public void AddScore(int score)
    {
        currentScore += score;
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }

    public int GetCorrectAnswers()
    {
        return currentScore;
    }

    public int GetQuestionSeen()
    {
        return questionSeen;
    }

    public void IncrementCorrectAnswers()
    {
        currentScore++;
    }

    public int CalculareScore()
    {
        if (questionSeen == 0) return 0;
        return Mathf.RoundToInt((float)currentScore / questionSeen * 100);
    }
}
