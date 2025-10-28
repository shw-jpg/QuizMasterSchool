using System.Collections;                  // ← 추가
using UnityEngine;

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
        lobbyCanvas.SetActive(true);
        quizCanvas.gameObject.SetActive(false);
        endScreen.gameObject.SetActive(false);
        loadingCanvas.SetActive(false);
    }

    public void StartGame()
    {
        lobbyCanvas.SetActive(false);
        quizCanvas.gameObject.SetActive(true);
        endScreen.gameObject.SetActive(false);
        loadingCanvas.SetActive(false);
        quizCanvas.StartQuiz();                // 무인자 오버로드 사용
    }

    public void ShowQuiz()
    {
        lobbyCanvas.SetActive(false);
        quizCanvas.gameObject.SetActive(true);
        endScreen.gameObject.SetActive(false);
        loadingCanvas.SetActive(false);
        quizCanvas.StartQuiz();
    }

    public void ShowEndScreen()
    {
        lobbyCanvas.SetActive(false);
        quizCanvas.gameObject.SetActive(false);
        endScreen.gameObject.SetActive(true);
        loadingCanvas.SetActive(false);
        endScreen.ShowFinalScore();
    }

    public void ReturnToLobby()
    {
        ShowLobby();
    }

    // 다시하기: 로딩 → StartCanvas(=lobbyCanvas)
    public void ReturnToStartCanvas()
    {
        StartCoroutine(LoadStartCanvasRoutine());
    }

    private IEnumerator LoadStartCanvasRoutine()
    {
        lobbyCanvas.SetActive(false);
        quizCanvas.gameObject.SetActive(false);
        endScreen.gameObject.SetActive(false);
        loadingCanvas.SetActive(true);

        yield return new WaitForSeconds(2f);

        loadingCanvas.SetActive(false);
        lobbyCanvas.SetActive(true);
    }
}
