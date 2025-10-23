using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Canvases")]
    [SerializeField] private GameObject lobbyCanvas;
    [SerializeField] private Quiz quizCanvas;
    [SerializeField] private EndScreen endScreen;
    [SerializeField] private GameObject loadingCanvas;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
    }



    // 로비 화면 표시
    public void ShowLobby()
    {
        lobbyCanvas.SetActive(true);  // 로비 켜기
        quizCanvas.gameObject.SetActive(false);
        endScreen.gameObject.SetActive(false);
        loadingCanvas.SetActive(false);
    }

    // 퀴즈 화면 표시
    public void ShowQuiz()
    {
        lobbyCanvas.SetActive(false);
        quizCanvas.gameObject.SetActive(true);
        endScreen.gameObject.SetActive(false);
        loadingCanvas.SetActive(false);

        quizCanvas.StartQuiz(); // 퀴즈 시작
    }

    // EndScreen 표시
    public void ShowEndScreen()
    {
        lobbyCanvas.SetActive(false);
        quizCanvas.gameObject.SetActive(false);
        endScreen.gameObject.SetActive(true);
        loadingCanvas.SetActive(false);

        endScreen.ShowFinalScore();
    }

    // 로비로 돌아가기
    public void ReturnToLobby()
    {
        ShowLobby();
    }
}
