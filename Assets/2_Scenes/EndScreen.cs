using System.Collections;                  // ← 추가
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI finalScoreText;
    [SerializeField] ScoreKeeper scoreKeeper;
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
        StartCoroutine(RestartSequence());
    }

    private IEnumerator RestartSequence()
    {
        scoreKeeper.ResetScore();
        GameManager.Instance.ReturnToStartCanvas(); // 로딩→StartCanvas
        yield break;
    }
}
