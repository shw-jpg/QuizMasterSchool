using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI finalScoreText;
    [SerializeField] ScoreKeeper scoreKeeper; // ScoreKeeper 타입으로 변경
    [SerializeField] Button replayButton;

    void Start()
    {
        replayButton.onClick.AddListener(OnReplayButtonClicked);
    }

    public void ShowFinalScore()
    {
        finalScoreText.text = "축하합니다!\n\n" +
            $"당신의 점수는 {scoreKeeper.GetCurrentScore()}점 입니다.";
    }

    private void OnReplayButtonClicked()
    {
        GameManager.Instance.ReturnToLobby(); // 로비로 이동
        scoreKeeper.ResetScore(); // 점수 초기화
    }
}
