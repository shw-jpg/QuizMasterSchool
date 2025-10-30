using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class ChatGPTRequest
{
    public string model = "gpt-4.1-nano";
    public Message[] messages;
    public float temperature = 1.1f;
    public int max_completion_tokens = 4000;
}

[Serializable]
public class Message
{
    public string role;
    public string content;
}

// OpenAI 응답 래퍼
[Serializable]
public class ChatGPTResponse
{
    public Choice[] choices;
}

[Serializable]
public class Choice
{
    public Message message;
}

// JSON 파싱용 QuizData 래퍼
[Serializable]
public class QuizDataWrapper
{
    public QuizQuestion[] questions;
}

[Serializable]
public class QuizQuestion
{
    public string question;
    public string[] answers;
    public int correctAnswerIndex;
}

public class ChatGPTClient : MonoBehaviour
{
    private const string API_URL = "https://api.openai.com/v1/chat/completions";
    private string apiKey;

    public delegate void QuizGenerateHandler(List<QuestionSO> questions);
    public event QuizGenerateHandler quizGenerateHandler;

    private void Awake()
    {
        apiKey = LoadFromResources();
    }

    private string LoadFromResources()
    {
        try
        {
            TextAsset configFile = Resources.Load<TextAsset>("config");
            if (configFile != null)
            {
                string[] lines = configFile.text.Split('\n');
                foreach (string line in lines)
                {
                    if (line.StartsWith("OPENAI_API_KEY="))
                        return line.Substring("OPENAI_API_KEY=".Length).Trim();
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Resources 설정 파일 로드 실패: {e.Message}");
        }

        return "";
    }

    public void GenerateQuizQuestions(int count = 3, string topic = "일반상식")
    {
        StartCoroutine(RequestQuizQuestions(count, topic));
    }

    private IEnumerator RequestQuizQuestions(int count, string topic)
    {
        string prompt = $"다음 조건에 맞는 객관식 퀴즈 문제를 {count}개 생성해주세요:\n" +
                        $"주제: {topic}\n" +
                        "조건:\n" +
                        "- 각 문제는 4개의 선택지를 가져야 합니다\n" +
                        "- 정답은 0~3 인덱스로 표시\n" +
                        "- 응답은 반드시 다음 JSON 형식으로 제공\n" +
                        "{ \"questions\": [ { \"question\": \"문제 내용\", \"answers\": [\"1\",\"2\",\"3\",\"4\"], \"correctAnswerIndex\": 0 } ] }";

        ChatGPTRequest request = new ChatGPTRequest
        {
            messages = new Message[] { new Message { role = "user", content = prompt } }
        };

        string jsonRequest = JsonUtility.ToJson(request);

        using (UnityWebRequest webRequest = new UnityWebRequest(API_URL, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonRequest);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Authorization", $"Bearer {apiKey}");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                string rawResponse = webRequest.downloadHandler.text;
                Debug.Log("Raw response: " + rawResponse);

                try
                {
                    ChatGPTResponse response = JsonUtility.FromJson<ChatGPTResponse>(rawResponse);
                    if (response?.choices == null || response.choices.Length == 0)
                    {
                        Debug.LogError("ChatGPT 응답 구조 오류");
                        yield break;
                    }

                    string content = response.choices[0].message.content.Trim();

                    // ```json 코드 블록 제거
                    if (content.StartsWith("```json")) content = content.Substring(7);
                    if (content.EndsWith("```")) content = content.Substring(0, content.Length - 3);
                    content = content.Trim();

                    // JSON 파싱
                    QuizDataWrapper quizData = JsonUtility.FromJson<QuizDataWrapper>(content);
                    List<QuestionSO> generatedQuestions = new List<QuestionSO>();
                    foreach (var q in quizData.questions)
                    {
                        QuestionSO so = ScriptableObject.CreateInstance<QuestionSO>();
                        so.SetData(q.question, q.answers, q.correctAnswerIndex);
                        generatedQuestions.Add(so);
                    }

                    quizGenerateHandler?.Invoke(generatedQuestions);
                }
                catch (Exception e)
                {
                    Debug.LogError($"응답 파싱 오류: {e.Message}");
                }
            }
            else
            {
                Debug.LogError($"ChatGPT API 요청 실패: {webRequest.error}");
            }
        }
    }

    public void SetApiKey(string key)
    {
        apiKey = key;
        PlayerPrefs.SetString("OpenAI_API_Key", key);
        PlayerPrefs.Save();
    }
}
