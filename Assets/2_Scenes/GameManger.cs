using UnityEngine;
using UnityEngine.SceneManagement;
using static StartMenu;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Quiz quiz;
    [SerializeField] private EndScreen endScreen;
    [SerializeField] private GameObject losdingCanves;

    public ChatGPTClient chatClient;
    private string[] currentQuestions;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    

    void Start()
    {
        //ShowQuizSceen();
        int category = QuizCategory.selectedCategory;
        string topic = GetTopicName(QuizCategory.selectedCategory);
        chatClient.GenerateQuizQuestions(5, topic);

        if (currentQuestions != null && currentQuestions.Length > 0)
        {
            Debug.Log("랜덤 문제: " + currentQuestions[Random.Range(0, currentQuestions.Length)]);
        }
    }

    private string GetTopicName(int selectedCategory)
    {
        switch (selectedCategory)
        {
            case 0: return "과학";
            case 1: return "역사";
            case 2: return "스포츠";
            case 3: return "영화";
            default:
                string[] topics = { "과학", "역사", "스포츠", "영화", "음악", "문학" };
                return topics[UnityEngine.Random.Range(0, topics.Length)];
        }
    }


    public void ShowQuizSceen()
    {
        quiz.gameObject.SetActive(true);
        endScreen.gameObject.SetActive(false);
        losdingCanves.SetActive(false);
    }

    public void ShowEndSceen()
    {
        quiz.gameObject.SetActive(false);
        endScreen.gameObject.SetActive(true);
        endScreen.ShowFinalScore();
        losdingCanves.SetActive(false);
    }

    public void ShowLoadingSceen()
    {
        quiz.gameObject.SetActive(false);
        endScreen.gameObject.SetActive(false);
        losdingCanves.SetActive(true);
    }

    public void OnReplayLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
