using UnityEngine;

[CreateAssetMenu(menuName = "Quiz Question", fileName = "New Question")]
public class QuestionSO : ScriptableObject
{
    [TextArea(2, 6)]
    [SerializeField] string question = "여기에 질문을 적어주세요";
    [SerializeField] string[] answers = new string[4];
    [SerializeField] int correctAnswerIndex = 0;
    [SerializeField] string hint = "빈값";

    public string GetQuestion()
    {
        return question;
    }

    public string Getanswer(int undex)
    {
        return answers[undex];
    }

    public string GetCorrectAnswer()
    {
        return answers[correctAnswerIndex];
    }

    public int GetCorrectAnswerIndex()
    {
        return correctAnswerIndex;
    }
    internal string GetHint()
    {
        return hint;
    }

    public void SetData(string q, string[] a, int corrctindex, string h)
    {
        SetData(q, a, corrctindex);
        hint = h;
    }
    public void SetData(string q, string[] a, int corrctindex)
    {
        question = q;
        answers = a;
        correctAnswerIndex = corrctindex;
    }
}
