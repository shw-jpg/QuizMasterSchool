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
    SocreKeeper scoreKeeper;

    [Header("ProgressBar")]
    [SerializeField] Slider progressBar;

    bool isGenerateQuestions = false;


    void Start()
    {
        timer = FindFirstObjectByType<Timer>();
        scoreKeeper = FindFirstObjectByType<SocreKeeper>();

        if (questions.Count <= 0)
        {
            GenerateQestionslfNeeded();
        }
        else
        {
            InitializeProgressBar();
        }
    }

    private void GenerateQestionslfNeeded()
    {
        if (isGenerateQuestions) return;

        isGenerateQuestions = true;
        GameManager.Instance.ShowLoadingSceen();
    }

    private void InitializeProgressBar()
    {
        progressBar.maxValue = questions.Count;
        progressBar.value = 0;
    }

    private void Update()
    {
        //Timer 이미지 업데이트
        if (timer.isProblemTime)
            timerImage.sprite = problemTimerSprite;
        else
            timerImage.sprite = solutionTimerSprite;
        timerImage.fillAmount = timer.fillAmount;

        //다음 질문 불러오기
        if (timer.loadNextQuestion)
        {
            if (questions.Count == 0)
            {
                GenerateQestionslfNeeded();
                //GameManager.Instance.ShowEndSceen();
            }
            else
            {
                timer.loadNextQuestion = false;
                GetNoxtQuestion();
            }
        }

        //SolutionTime이고 답을 선택하지 않았을 때
        if (timer.isProblemTime == false && chooseAnswer == false)
        {
            DisplaySolution(-1);
        }

    }

    private void GetNoxtQuestion()
    {
        Debug.Log("GameManager ShowEndSceen");
        if (questions.Count <= 0)
        {
            Debug.Log("남은 문제가 없습니다."); 
            return;
        }

        GameManager.Instance.ShowQuizSceen();
        chooseAnswer = false;
        SetButtoState(true);
        SetDefsultButtonSprites();
        GetRsndomQuesion();
        OnDisplayQuestion();
        scoreKeeper.IncrementQuestionSeen();
        progressBar.value++;
    }

    private void GetRsndomQuesion()
    {
        int randomlndex = UnityEngine.Random.Range(0, questions.Count);
        currentQuestion = questions[randomlndex];

        questions.RemoveAt(randomlndex);
    }

    private void OnDisplayQuestion()
    {
        questionText.text = currentQuestion.GetQuestion();

        for (int i = 0; i < answerButtonArr.Length; i++)
        {
            answerButtonArr[i].GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.Getanswer(i);
        }
    }

    public void OnanswerButtonClicked(int index)
    {
        chooseAnswer = true;
        DisplaySolution(index);
        timer.CancelTimer();
        scoreText.text = $"Score:{scoreKeeper.CalculareScore()}%";
    }

    private void DisplaySolution(int index)
    {
        if (index == currentQuestion.GetCorrectAnswerIndex())
        {
            questionText.text = "정답입니다!";
            answerButtonArr[index].GetComponent<Image>().sprite = correctAnswerSprite;
            scoreKeeper.IncrementCorrectAnswers();
        }
        else
        {
            questionText.text = "틀렸습니다!" + currentQuestion.GetCorrectAnswer();
        }
        SetButtoState(false);
    }

    private void SetDefsultButtonSprites()
    {
        foreach (GameObject obj in answerButtonArr)
        {
            obj.GetComponent<Image>().sprite = defaultAnswerSprite;
        }
    }

    private void SetButtoState(bool state)
    {
        foreach (GameObject obj in answerButtonArr)
        {
            obj.GetComponent<Button>().interactable = state;
        }
    }
}
