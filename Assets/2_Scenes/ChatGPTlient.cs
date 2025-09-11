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
        quizGerateHandier?.Invoke(new List<QuestionSO>());
        Debug.Log("Finished GenerateWithDelay...........");

    }
}
