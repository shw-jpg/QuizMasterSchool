using UnityEngine;

[CreateAssetMenu(menuName = "Quiz Question", fileName = "New Question")]
public class QuestionSO : ScriptableObject
{
    [TextArea(2, 6)]
    [SerializeField] string question = "여기에 질문을 적어주세요";
    [SerializeField] string[] answers = new string[4];
    [SerializeField] int correctAnswerlndex;

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
        return answers[correctAnswerlndex];
    }

    public int GetCorrectAnswerIndex()
    {
        return correctAnswerlndex;
    }
}
