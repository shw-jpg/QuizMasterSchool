using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    [Header("질문")]
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] List<QuestionSO> questions = new List<QuestionSO>();
    QuestionSO currentQuestion;

    [Header("보기")]
    [SerializeField] GameObject[] answerButtonArr;

    [Header("버튼 색깔")]
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;

    [Header("Timer")]
    [SerializeField] Image timerImage;
    [SerializeField] Sprite problemTimerSprite;
    [SerializeField] Sprite solutionTimerSprite;
    Timer timer;
    bool chooseAnswer = false;

    [Header("점수")]
    [SerializeField] TextMeshProUGUI scoreText;
    ScoreKeeper scoreKeeper;

    [Header("ProgressBar")]
    [SerializeField] Slider progressBar;

    [Header("ChatGPTClient")]
    [SerializeField] ChatGPTClient chatGPTClinet;
    [SerializeField] int questionCount = 3;
    [SerializeField] TextMeshProUGUI loadingText;
    bool isGenerateQuestions = false;

    [Header("힌트")]
    [SerializeField] Button hintButton;
    [SerializeField] TextMeshProUGUI hintText;

    [Header("Apple Slot")]
    [SerializeField] AppleSlotDisplay appleSlotDisplay;

    [Header("종료 UI")]
    [SerializeField] GameObject endPanel;
    [SerializeField] Button restartButton;

    void Start()
    {
        timer = FindFirstObjectByType<Timer>();
        scoreKeeper = FindFirstObjectByType<ScoreKeeper>();

        chatGPTClinet.quizGenerateHandler += QuizGeneratedHandler;

        hintButton.onClick.AddListener(ShowHint);
        hintText.gameObject.SetActive(false);

        restartButton.onClick.AddListener(RestartQuiz);

        endPanel.SetActive(false); // 시작 시 종료 UI 숨김

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

        GameManager.Instance.ShowQuiz(); // 또는 GameManager.Instance.ShowLobby();로 변경

        string topicToUse = StartCanvas.QuizCategory.selectedCategory >= 0
            ? GetTopicName(StartCanvas.QuizCategory.selectedCategory)
            : GetTrendingTopic();

        chatGPTClinet.GenerateQuizQuestions(questionCount, topicToUse);
    }

    private string GetTrendingTopic()
    {
        string[] topics = { "과학", "역사", "스포츠", "영화", "음악", "문학", "기술", "지리", "예술", "동물", "음식" };
        int randomIndex = Random.Range(0, topics.Length);
        return topics[randomIndex];
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

    public void StartQuiz(int selectedCategory)
    {
        Debug.Log("선택한 카테고리: " + selectedCategory);
        StartCanvas.QuizCategory.selectedCategory = selectedCategory;

        // 문제 생성 및 첫 질문 불러오기
        if (questions.Count <= 0)
            GenerateQuestionsIfNeeded();
        else
        {
            InitializeProgressBar();
            GetNextQuestion();
        }
    }

    void QuizGeneratedHandler(List<QuestionSO> generatedQuestions)
    {
        isGenerateQuestions = false;

        if (generatedQuestions == null || generatedQuestions.Count == 0)
        {
            loadingText.text = "문제 생성 실패!\n인터넷 연결 확인 후 재시도.";
            return;
        }

        questions.AddRange(generatedQuestions);
        progressBar.maxValue = questions.Count;

        InitializeProgressBar();
        GetNextQuestion();
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
            OnAllQuestionsFinished();
            ShowEndPanel();
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

        // 다음 문제 호출
        Invoke(nameof(GetNextQuestion), 1.0f); // 1초 후 다음 문제
    }

    private void DisplaySolution(int index)
    {
        if (index == currentQuestion.GetCorrectAnswerIndex())
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

    private void ShowEndPanel()
    {
        endPanel.SetActive(true);
    }

    private void RestartQuiz()
    {
        // 사과, 점수, 진행바 초기화
        appleSlotDisplay.ResetApples();
        scoreKeeper = FindFirstObjectByType<ScoreKeeper>();
        scoreKeeper.GetCurrentScore();

        progressBar.value = 0;
        endPanel.SetActive(false);

        GameManager.Instance.ShowLobby(); // 로비 화면으로 돌아가기
    }

    public void StartQuiz()
    {
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

    private void OnAllQuestionsFinished()
    {
        GameManager.Instance.ShowEndScreen();
    }

    public int GetScore()
    {
        return scoreKeeper.GetCurrentScore();
    }
}
