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

    void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
    }

    public void ShowLobby()
    {
        lobbyCanvas.SetActive(true);
        if (quizCanvas != null) quizCanvas.SetActive(false);
        if (endScreen != null) endScreen.gameObject.SetActive(false);
        if (loadingCanvas != null) loadingCanvas.SetActive(false);
    }

    public void ShowQuizScreen()
    {
        if (quiz != null) quiz.gameObject.SetActive(true);
        if (endScreen != null) endScreen.gameObject.SetActive(false);
        if (loadingCanvas != null) loadingCanvas.SetActive(false);
    }

    public void ShowEndScreen()
    {
        if (quiz != null) quiz.gameObject.SetActive(false);
        if (endScreen != null)
        {
            endScreen.gameObject.SetActive(true);
            endScreen.ShowFinalScore();
        }
        if (loadingCanvas != null) loadingCanvas.SetActive(false);
    }

    public void ShowLoadingScreen()
    {
        if (quiz != null) quiz.gameObject.SetActive(false);
        if (endScreen != null) endScreen.gameObject.SetActive(false);
        if (loadingCanvas != null) loadingCanvas.SetActive(true);
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
        if (lobbyCanvas != null) lobbyCanvas.SetActive(false);
        if (quizCanvas != null) quizCanvas.SetActive(false);
        if (endScreen != null) endScreen.gameObject.SetActive(false);
        if (loadingCanvas != null) loadingCanvas.SetActive(true);

        yield return new WaitForSeconds(2f);

        if (loadingCanvas != null) loadingCanvas.SetActive(false);
        if (lobbyCanvas != null) lobbyCanvas.SetActive(true);
    }
}
