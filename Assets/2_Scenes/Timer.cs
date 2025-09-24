using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] float problemTime = 10f;
    [SerializeField] float solutionTime = 3f;
    float time = 0;

    [HideInInspector] public bool isProblemTime = true;
    [HideInInspector] public float fillAmount;
    [HideInInspector] public bool loadNextQuestion;

    [Header("UIÇ¥½Ã")]
    [SerializeField] TextMeshProUGUI timerText;

    private void Start()
    {
        time = problemTime;
        loadNextQuestion = true;
    }

    private void Update()
    {
        TimerCountDown();
        UpdateFillAmount();
        UpdateTimerText();
    }

    private void UpdateFillAmount()
    {
        if (isProblemTime)
            fillAmount = time / problemTime;

        else
            fillAmount = time / solutionTime;
    }


    private void TimerCountDown()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            if (isProblemTime)
            {
                isProblemTime = false;
                time = solutionTime;
            }
            else
            {
                isProblemTime = true;
                time = problemTime;
                loadNextQuestion = true;
            }
        }
    }

    private void UpdateTimerText()
    {
        int displayTime = Mathf.CeilToInt(time);

        timerText.text = displayTime.ToString();
    }

    public void CancelTimer()
    {
        time = 0;
    }
}
