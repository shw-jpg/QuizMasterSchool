using Microsoft.Unity.VisualStudio.Editor;
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

    // ... 기존 코드 생략 ...

    [Header("UI표시")]
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] private UnityEngine.UI.Image timerImage; // Image 타입을 UnityEngine.UI.Image로 변경

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
        TimeColor();
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

    // ... 기존 코드 생략 ...

    public void TimeColor()
    {
        if (time > 7f)
        {
            timerImage.color = Color.green;
        }
        else if (time > 3f)
        {
            timerImage.color = Color.yellow;
        }
        else
        {
            timerImage.color = Color.red;
        }
    }
}
