using System.Collections;
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
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;
    [SerializeField] Sprite selectedAnswerSprite;

    [Header("Timer")]
    Timer timer;

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

    void Start()
    {
        timer = FindFirstObjectByType<Timer>();
        scoreKeeper = FindFirstObjectByType<ScoreKeeper>();
        chatGPTClinet.quizGenerateHandler += QuizGeneratedHandler;

        hintButton.onClick.AddListener(ShowHint);
        hintText.gameObject.SetActive(false);

        if (questions.Count <= 0)
            GenerateQuestionsIfNeeded();
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

        string topicToUse = StartCanvas.QuizCategory.selectedCategory >= 0
            ? GetTopicName(StartCanvas.QuizCategory.selectedCategory)
            : GetTrendingTopic();

        chatGPTClinet.GenerateQuizQuestions(questionCount, topicToUse);
    }

    private string GetTrendingTopic()
    {
        string[] topics = { "과학", "역사", "스포츠", "영화", "음악", "문학", "기술", "지리", "예술", "동물", "음식" };
        return topics[Random.Range(0, topics.Length)];
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
        Debug.Log($"선택된 카테고리: {selectedCategory}");
        StartCanvas.QuizCategory.selectedCategory = selectedCategory;

        questions.Clear();
        progressBar.value = 0;

        GenerateQuestionsForCategory(selectedCategory);
    }

    private void GenerateQuestionsForCategory(int selectedCategory)
    {
        string topic = GetTopicName(selectedCategory);
        chatGPTClinet.GenerateQuizQuestions(questionCount, topic);
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
            GameManager.Instance.ShowEndScreen();
            return;
        }

        currentQuestion = questions[0];
        questions.RemoveAt(0);

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
            answerButtonArr[i].GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.Getanswer(i);

        timer.StartProblemPhase();  // 10초 타이머 시작
    }

    public void OnAnswerButtonClicked(int index)
    {
        int remain = timer.GetRemainingWholeSeconds(); // 남은 초

        DisplaySolution(index);
        timer.CancelTimer();
        timer.Pausetimer();

        scoreText.text = $"Score:{scoreKeeper.GetCurrentScore()}점";
        Invoke(nameof(GetNextQuestion), 1.0f);
    }

    private void DisplaySolution(int index)
    {
        if (index == currentQuestion.GetCorrectAnswerIndex())
        {
            questionText.text = "정답입니다!";

            // 정답 버튼을 주황색으로 변경
            answerButtonArr[index].GetComponent<Image>().sprite = selectedAnswerSprite;

            scoreKeeper.IncrementCorrectAnswers();

            int bonus = timer.GetRemainingWholeSeconds();
            scoreKeeper.AddScore(bonus);

            appleSlotDisplay.AddApple();
        }
        else
        {
            questionText.text = "틀렸습니다! 정답: " + currentQuestion.GetCorrectAnswer();

            // 정답 버튼 찾아서 주황색 표시
            int correct = currentQuestion.GetCorrectAnswerIndex();
            answerButtonArr[correct].GetComponent<Image>().sprite = selectedAnswerSprite;

            appleSlotDisplay.RemoveApple();
        }

        SetButtonState(false);
        timer.StartSolutionPhase(); // 정답 표시 시간 유지
    }

    private void SetDefaultButtonSprites()
    {
        foreach (GameObject obj in answerButtonArr)
            obj.GetComponent<Image>().sprite = defaultAnswerSprite;
    }

    private void SetButtonState(bool state)
    {
        foreach (GameObject obj in answerButtonArr)
            obj.GetComponent<Button>().interactable = state;
    }

    private void OnAllQuestionsFinished()
    {
        StartCoroutine(ShowLoadingThenEnd());
    }

    private IEnumerator ShowLoadingThenEnd()
    {
        GameManager.Instance.ShowEndScreen();
        yield return null;
    }

    private void ShowHint()
    {
        string h = currentQuestion.GetHint();
        if (string.IsNullOrEmpty(h) || h == "빈값")
        {
            string ans = currentQuestion.GetCorrectAnswer();
            string fallback = ans.Length > 1 ? $"{ans[0]}… ({ans.Length}글자)" : ans;
            h = $"정답 힌트: {fallback}";
        }

        hintText.text = h;
        hintText.gameObject.SetActive(true);
    }

    public void StartQuiz()
    {
        StartQuiz(0);
    }
}
