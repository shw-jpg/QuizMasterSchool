using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Quiz quiz;
    [SerializeField] private EndScreen endScreen;
    [SerializeField] private GameObject losdingCanves;


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
