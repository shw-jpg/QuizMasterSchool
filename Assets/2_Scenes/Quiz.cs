using TMPro;
using UnityEngine;

public class ㅃ : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] QuestionSO question;
    [SerializeField] TextMeshProUGUI[] answerTextArr;

    void Start()
    {
        questionText.text = question.GetQuestion();
                
        for (int i = 0; i < answerTextArr.Length; i++)
        {
            answerTextArr[i].text = question.Getanswer(i);
        }
    }
}
