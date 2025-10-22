using TMPro;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI finalScoreText;
    [SerializeField] ScoreKeeper scoreKeeper; // ScoreKeeper 타입으로 변경

    public void ShowFinalScore()
    {
        finalScoreText.text = "축하합니다!\n\n" +
            $"당신의 점수는 {scoreKeeper.GetCurrentScore()}점 입니다.";
    }
}
