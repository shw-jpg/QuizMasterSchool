using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    [Header("질문")]
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private List<QuestionSO> questions = new List<QuestionSO>();
    private QuestionSO currentQuestion;

    [Header("보기")]
    [SerializeField] private GameObject[] answerButtonArr;

    [Header("버튼 색깔")]
    [SerializeField] private Sprite defaultAnswerSprite; // 파란색
    [SerializeField] private Sprite correctAnswerSprite; // 주황색

    [Header("Timer")]
    [SerializeField] private Image timerImage;
    private Timer timer;

    [Header("점수")]
    [SerializeField] private TextMeshProUGUI scoreText;
    private ScoreKeeper scoreKeeper;

    [Header("ProgressBar")]
    [SerializeField] private Slider progressBar;

    [Header("ChatGPTClient")]
    [SerializeField] private ChatGPTClient chatGPTClinet;
    [SerializeField] private int questionCount = 3;
    [SerializeField] private TextMeshProUGUI loadingText;
    private bool isGenerateQuestions = false;

    [Header("힌트")]
    [SerializeField] private Button hintButton;
    [SerializeField] private TextMeshProUGUI hintText;

    [Header("Apple Slot")]
    [SerializeField] private AppleSlotDisplay appleSlotDisplay;

    [Header("Canvas")]
    [SerializeField] private GameObject winCanvas;

    private bool chooseAnswer = false;

    void Start()
    {
        timer = FindFirstObjectByType<Timer>();
        scoreKeeper = FindFirstObjectByType<ScoreKeeper>();
        chatGPTClinet.quizGenerateHandler += QuizGeneratedHandler;

        if (winCanvas != null)
            winCanvas.SetActive(false);

        hintButton.onClick.AddListener(ShowHint);
        hintText.gameObject.SetActive(false);

        if (questions.Count <= 0)
        {
            GenerateQuestionsIfNeeded();
        }
        else
        {
            InitializeProgressBar();
            GetNextQuestion();
        }
    }

    private void GenerateQuestionsIfNeeded()
    {
        if (isGenerateQuestions) return;
        isGenerateQuestions = true;

        GameManager.Instance.ShowLoadingScreen();

        int selectedCategory = StartCanvas.QuizCategory.selectedCategory;
        string topicToUse = GetTopicName(selectedCategory);

        chatGPTClinet.GenerateQuizQuestions(questionCount, topicToUse);
        Debug.Log($"GenerateQuestionsIfNeeded: {topicToUse}");
    }

    private string GetTopicName(int selectedCategory)
    {
        switch (selectedCategory)
        {
            case 0: return "과학";
            case 1: return "역사";
            case 2: return "스포츠";
            case 3: return "영화";
            case 4: return "음악";
            default: return "일반상식";
        }
    }

    private void QuizGeneratedHandler(List<QuestionSO> generatedQuestions)
    {
        isGenerateQuestions = false;

        if (generatedQuestions == null || generatedQuestions.Count == 0)
        {
            loadingText.text = "문제 생성 실패!\n인터넷 연결 확인 후 재시도.";
            return;
        }

        questions.Clear();
        questions.AddRange(generatedQuestions);

        InitializeProgressBar();
        GetNextQuestion();

        GameManager.Instance.ShowQuizScreen();
    }

    private void InitializeProgressBar()
    {
        progressBar.maxValue = questions.Count;
        progressBar.value = 0;
    }

    private void GetNextQuestion()
    {
        if (questions.Count <= 0)
        {
            ShowWinCanvas();
            return;
        }

        currentQuestion = questions[0];
        questions.RemoveAt(0);

        chooseAnswer = false;
        SetButtonState(true);
        SetDefaultButtonSprites();
        DisplayQuestion();
        progressBar.value++;
    }

    private void DisplayQuestion()
    {
        if (currentQuestion == null) return;

        questionText.text = currentQuestion.GetQuestion();
        hintText.gameObject.SetActive(false);

        for (int i = 0; i < answerButtonArr.Length; i++)
        {
            answerButtonArr[i].GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.Getanswer(i);
        }
    }

    public void OnAnswerButtonClicked(int index)
    {
        chooseAnswer = true;
        DisplaySolution(index);
        timer.CancelTimer();
        scoreText.text = $"Score:{scoreKeeper.GetCurrentScore()}점";

        Invoke(nameof(GetNextQuestion), 1.5f);
    }

    private void DisplaySolution(int index)
    {
        int correctIndex = currentQuestion.GetCorrectAnswerIndex();

        if (index == correctIndex)
        {
            questionText.text = "정답입니다!";
            answerButtonArr[index].GetComponent<Image>().sprite = correctAnswerSprite;
            scoreKeeper.IncrementCorrectAnswers();
            appleSlotDisplay.AddApple();
        }
        else
        {
            questionText.text = "틀렸습니다! 정답: " + currentQuestion.GetCorrectAnswer();
            appleSlotDisplay.RemoveApple();
        }

        SetButtonState(false);
    }

    private void SetDefaultButtonSprites()
    {
        foreach (GameObject obj in answerButtonArr)
        {
            obj.GetComponent<Image>().sprite = defaultAnswerSprite;
        }
    }

    private void SetButtonState(bool state)
    {
        foreach (GameObject obj in answerButtonArr)
        {
            obj.GetComponent<Button>().interactable = state;
        }
    }

    private void ShowHint()
    {
        hintText.text = "힌트: " + currentQuestion.GetHint();
        hintText.gameObject.SetActive(true);
    }

    private void ShowWinCanvas()
    {
        if (winCanvas != null)
            winCanvas.SetActive(true);
    }

    public void StartQuiz(int selectedCategory)
    {
        Debug.Log($"[Quiz] StartQuiz 실행됨 — 선택된 카테고리: {selectedCategory}");

        // 선택 카테고리 저장
        StartCanvas.QuizCategory.selectedCategory = selectedCategory;

        // 문제 생성
        GenerateQuestionsIfNeeded();

        // WinCanvas 숨기기
        if (winCanvas != null)
            winCanvas.SetActive(false);
    }
}
