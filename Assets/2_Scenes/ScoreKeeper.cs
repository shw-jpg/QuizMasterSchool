using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    int currentScore = 0;
    int questionSeen = 0;

    public int GetCorrectAnswers()
    {
        return currentScore;
    }

    public void IncrementCorrectAnswers()
    {
        currentScore++;
    }

    public int GetQuestionSeen()
    {
        return questionSeen;
    }

    public void IncrementQuestionSeen()
    {
        questionSeen++;
    }

    public int CalculareScore()
    {
        if (questionSeen == 0) return 0;
        return Mathf.RoundToInt((float)currentScore / questionSeen * 100);
    }

    public void AddScore(int score)
    {
        currentScore += score;
    }
}
