using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ChatGPTClinet;

public class ChatGPTClinet : MonoBehaviour
{
    public delegate void QuizGeneratedHandler(List<QuestionSO>questions);
    public event QuizGeneratedHandler quizGerateHandier;

    internal void GenerateQuestions(int questionCount, string topicToUse)
    {
        Debug.Log($"Generating {questionCount}questiions on the topic: {topicToUse}");

        StartCoroutine(GenerateWithDelay());
    }

    private IEnumerator GenerateWithDelay()
    {
        yield return new WaitForSeconds(3f);
        List<QuestionSO> questions = new List<QuestionSO>();
        QuestionSO so1 = CreateQuesion("GPT 생성 질문1",
            new string[] { "답변1", "답변2", "답변3", "답변4" },
            0);
        questions.Add(so1);
        QuestionSO so2 = CreateQuesion("GPT 생성 질문2",
            new string[] { "답변1", "답변2", "답변3", "답변4" },
            1);
        questions.Add(so2);
        QuestionSO so3 = CreateQuesion("GPT 생성 질문3",
            new string[] { "답변1", "답변2", "답변3", "답변4" },
            2);
        questions.Add(so3);

        quizGerateHandier?.Invoke(questions);
        Debug.Log("Finished GenerateWithDelay...........");

    }

    QuestionSO CreateQuesion(string q, string[] answers, int correctIndex)
    {
        QuestionSO so = ScriptableObject.CreateInstance<QuestionSO>();
        so.SerData(q, answers, correctIndex);

        return so;
    }
}
