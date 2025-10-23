using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    int currentScore = 0;
    int correctAnswers = 0;
    int questionSeen = 0;

    public void AddScore(int score)
    {
        currentScore += score;
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }

    public void IncrementCorrectAnswers()
    {
        correctAnswers++;
        currentScore += 1;
    }

    public int GetCorrectAnswers()
    {
        return correctAnswers;
    }

    public void IncrementQuestionsSeen()
    {
        questionSeen++;
    }

    public int GetQuestionSeen()
    {
        return questionSeen;
    }

    public int CalculateScore()
    {
        if (questionSeen == 0) return 0;
        return Mathf.RoundToInt((float)currentScore / questionSeen * 100);
    }

    public void ResetScore()
    {
        currentScore = 0;
        correctAnswers = 0;
        questionSeen = 0;
    }
}
