using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] float problemTime = 10f;
    [SerializeField] float solutionTime = 3f;
    float time = 0;

    [HideInInspector] public bool isProblemTime = false;
    [HideInInspector] public float fillAmount; 

    private void Start()
    {
        time = problemTime;
    }

    private void Update()
    {
        timerCountDown();
        UpdateFillAmount();
    }

    private void timerCountDown()
    {
        Debug.Log("Time remaining: " + time);
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

    private void UpdateFillAmount()
    {
        if (isProblemTime)
            fillAmount = time / problemTime;

        else
            fillAmount = time / solutionTime;
    }
}
