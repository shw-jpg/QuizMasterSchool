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
    [SerializeField] private Button hintButton;
    [SerializeField] private TextMeshProUGUI hintText;

    [Header("Apple Slot")]
    [SerializeField] AppleSlotDisplay appleSlotDisplay;

    void Start()
    {
        timer = FindFirstObjectByType<Timer>();
        scoreKeeper = FindFirstObjectByType<ScoreKeeper>();

        chatGPTClinet.quizGenerateHandler += QuizGeneratedHandler;

        hintButton.onClick.AddListener(ShowHint);
        hintText.gameObject.SetActive(false); // 처음에는 숨김

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

        GameManager.Instance.ShowLoadingSceen();

        string topicToUse = StartCanvas.QuizCategory.selectedCategory >= 0
            ? GetTopicName(StartCanvas.QuizCategory.selectedCategory)
            : GetTrendingTopic();

        chatGPTClinet.GenerateQuizQuestions(questionCount, topicToUse);
        Debug.Log($"GenerateQuestionsIfNeeded {topicToUse}");
    }

    private string GetTrendingTopic()
    {
        string[] topics = { "과학", "역사", "음악", "영화", "스포츠", "기술", "문학", "지리", "예술", "동물", "음식" };
        int randomIndex = UnityEngine.Random.Range(0, topics.Length);
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
            Debug.Log("남은 문제가 없습니다.");
            return;
        }

        currentQuestion = questions[0]; // 문제 고정
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
        hintText.gameObject.SetActive(false); // 힌트 숨김

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
}
