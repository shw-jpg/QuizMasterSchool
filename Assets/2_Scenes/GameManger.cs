using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Canvases")]
    [SerializeField] private GameObject lobbyCanvas;
    [SerializeField] private GameObject startCanvas;
    [SerializeField] private GameObject quizCanvas;
    [SerializeField] private GameObject winCanvas;
    [SerializeField] private GameObject loadingCanvas;
    [SerializeField] private Quiz quiz;
    [SerializeField] private EndScreen endScreen;
    [SerializeField] private GameObject losdingCanves; // 오타 있음 (아래 참고)

    // 👇 이 줄은 삭제해야 함 (중복 선언)
    // [SerializeField] private Quiz quiz;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
    }

    public void ShowLobby()
    {
        lobbyCanvas.SetActive(true);
        quizCanvas.gameObject.SetActive(false);
        endScreen.gameObject.SetActive(false);
        loadingCanvas.SetActive(false);
    }

    public void ShowQuizScreen()
    {
        quiz.gameObject.SetActive(true);
        endScreen.gameObject.SetActive(false);
        losdingCanves.SetActive(false);
    }

    public void ShowEndScreen()
    {
        quiz.gameObject.SetActive(false);
        endScreen.gameObject.SetActive(true);
        endScreen.ShowFinalScore();
        losdingCanves.SetActive(false);
    }

    public void ShowLoadingScreen()
    {
        quiz.gameObject.SetActive(false);
        endScreen.gameObject.SetActive(false);
        losdingCanves.SetActive(true);
    }

    public void ReturnToLobby()
    {
        ShowLobby();
    }

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
