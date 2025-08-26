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
        GetNxtQuestion();
    }

    private void GetNxtQuestion()
    {
        OnDisplayQuestion();
        SetDefsultButtonSprites();
        SetButtoState(true);
    }

    private void OnDisplayQuestion()
    {
        questionText.text = question.GetQuestion();

        for (int i = 0; i < answerButtonArr.Length; i++)
        {
            answerButtonArr[i].GetComponentInChildren<TextMeshProUGUI>().text = question.Getanswer(i);
        }
    }

    public void OnanswerButtonClicked(int index)
    {
        if (index == question.GetCorrectAnswerIndex())
        {
            questionText.text = "정답입니다!";
            answerButtonArr[index].GetComponent<Image>().sprite = correctAnswerSprite;
        }
        else
        {
            questionText.text = "틀렸습니다!" + question.GetCorrectAnswer();
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
