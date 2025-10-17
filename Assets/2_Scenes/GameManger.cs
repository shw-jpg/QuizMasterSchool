using UnityEngine;
using UnityEngine.SceneManagement;
using static StartCanvas;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Quiz quiz;
    [SerializeField] private EndScreen endScreen;
    [SerializeField] private GameObject losdingCanves;
    public ChatGPTClient chatClient;
   


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
        string topic = GetTopicName(category);
        chatClient.GenerateQuizQuestions(5, topic);
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

    public void ReplayLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
