using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] float problemTime = 10f;
    [SerializeField] float solutionTime = 3f;
    float time = 0;

    [HideInInspector] public bool isProblemTime = true;

    [Header("UI표시")]
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] private UnityEngine.UI.Image timerImage;

    private bool isPaused = true;

    private void Start()
    {
        time = problemTime;
    }

    private void Update()
    {
        if (isPaused) return;
        TimerCountDown();
        UpdateTimerText();
        TimeColor(); // 그라데이션
    }

    public void StartProblemPhase()
    {
        isProblemTime = true;
        time = problemTime;
        isPaused = false;
    }

    public void StartSolutionPhase()
    {
        isProblemTime = false;
        time = solutionTime;
        isPaused = false;
    }

    public int GetRemainingWholeSeconds()
    {
        return Mathf.CeilToInt(time);
    }

    public void Resumetimer() { isPaused = false; }
    public void Pausetimer() { isPaused = true; }
    public void CancelTimer() { time = 0; }

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
            }
        }
    }

    private void UpdateTimerText()
    {
        timerText.text = Mathf.CeilToInt(time).ToString();
    }

    private void TimeColor()
    {
        float total = isProblemTime ? problemTime : solutionTime;
        float t = Mathf.Clamp01(time / total);
        Color c = (t > 0.5f)
            ? Color.Lerp(Color.yellow, Color.green, (t - 0.5f) / 0.5f)
            : Color.Lerp(Color.red, Color.yellow, t / 0.5f);
        timerImage.color = c;
    }
}
