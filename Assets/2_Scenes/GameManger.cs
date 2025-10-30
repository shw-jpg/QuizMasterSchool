using System.Collections;                  // ← 추가
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
<<<<<<< Updated upstream
    [SerializeField] private GameObject losdingCanves;


    [Header("Quiz Reference")]
    [SerializeField] private Quiz quiz;
=======
>>>>>>> Stashed changes

    void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
    }

    // 로비 화면 표시
    public void ShowLobby()
    {
        lobbyCanvas.SetActive(true);
        if (quizCanvas != null) quizCanvas.SetActive(false);
        if (endScreen != null) endScreen.gameObject.SetActive(false);
        if (loadingCanvas != null) loadingCanvas.SetActive(false);
    }

<<<<<<< HEAD
    public void ShowQuizScreen()
=======
    public void StartGame()
    {
        lobbyCanvas.SetActive(false);
        quizCanvas.gameObject.SetActive(true);
        endScreen.gameObject.SetActive(false);
        loadingCanvas.SetActive(false);
        quizCanvas.StartQuiz();                // 무인자 오버로드 사용
    }

    public void ShowQuiz()
>>>>>>> 2e1b27fbb027c4887117ed51c13b92d4fd5ffbfa
    {
<<<<<<< Updated upstream
        quiz.gameObject.SetActive(true);
        endScreen.gameObject.SetActive(false);
<<<<<<< HEAD
        losdingCanves.SetActive(false);
=======
        loadingCanvas.SetActive(false);
        quizCanvas.StartQuiz();
>>>>>>> 2e1b27fbb027c4887117ed51c13b92d4fd5ffbfa
=======
        if (quiz != null) quiz.gameObject.SetActive(true);
        if (endScreen != null) endScreen.gameObject.SetActive(false);
        if (loadingCanvas != null) loadingCanvas.SetActive(false);
>>>>>>> Stashed changes
    }

    public void ShowEndScreen()
    {
<<<<<<< Updated upstream
        quiz.gameObject.SetActive(false);
        endScreen.gameObject.SetActive(true);
<<<<<<< HEAD
        endScreen.ShowFinalScore();  // 점수 표시
        losdingCanves.SetActive(false);
=======
        if (quiz != null) quiz.gameObject.SetActive(false);
        if (endScreen != null)
        {
            endScreen.gameObject.SetActive(true);
            endScreen.ShowFinalScore();
        }
        if (loadingCanvas != null) loadingCanvas.SetActive(false);
>>>>>>> Stashed changes
    }

    public void ShowLoadingScreen()
    {
<<<<<<< Updated upstream
        quiz.gameObject.SetActive(false);
        endScreen.gameObject.SetActive(false);
        losdingCanves.SetActive(true);
=======
        loadingCanvas.SetActive(false);
        endScreen.ShowFinalScore();
>>>>>>> 2e1b27fbb027c4887117ed51c13b92d4fd5ffbfa
=======
        if (quiz != null) quiz.gameObject.SetActive(false);
        if (endScreen != null) endScreen.gameObject.SetActive(false);
        if (loadingCanvas != null) loadingCanvas.SetActive(true);
>>>>>>> Stashed changes
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
        if (lobbyCanvas != null) lobbyCanvas.SetActive(false);
        if (quizCanvas != null) quizCanvas.SetActive(false);
        if (endScreen != null) endScreen.gameObject.SetActive(false);
        if (loadingCanvas != null) loadingCanvas.SetActive(true);

        yield return new WaitForSeconds(2f);

        if (loadingCanvas != null) loadingCanvas.SetActive(false);
        if (lobbyCanvas != null) lobbyCanvas.SetActive(true);
    }
}
