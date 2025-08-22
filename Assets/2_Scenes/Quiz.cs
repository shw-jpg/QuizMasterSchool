using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] QuestionSO question;
    [SerializeField] GameObject[] answerButtonArr;
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;

    void Start()
    {
        questionText.text = question.GetQuestion();

        for (int i = 0; i < answerButtonArr.Length; i++)
        {
            answerButtonArr[i].GetComponentInChildren<TextMeshProUGUI>().text = question.Getanswer(i);
        }
    }

    public void OnanswerButtonClicled(int index)
    {
        if (index == question.GetCorrectAnswerIndex())
        {
            questionText.text = "정답입니다!";
            answerButtonArr[index].GetComponent<Image>().sprite = correctAnswerSprite;
        }
    }
}
