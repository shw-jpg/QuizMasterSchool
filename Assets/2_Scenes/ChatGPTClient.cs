using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

[Serializable]
public class ChatGPTRequest
{
    public string model = "gpt-4.1-nano";
    public Message[] messages;
    public float temperature = 1.1f;
    public int max_tokens = 4000; // max_completion_tokens 대신 max_tokens
}

[Serializable]
public class Message
{
    public string role;
    public string content;
}

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

[Serializable]
public class QuizData
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
        apiKey = LoadApiKey();
        if (string.IsNullOrEmpty(apiKey))
            Debug.LogError("OpenAI API Key가 비어있습니다. Resources/config.txt 확인 필요!");
    }

    private string LoadApiKey()
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
        string prompt = $"다음 조건에 맞는 창의적이고 재미있는 객관식 퀴즈 문제를 {count}개 생성해주세요:\n" +
                        $"주제: {topic}\n" +
                        "조건:\n" +
                        "- 각 문제는 4개의 선택지\n" +
                        "- 문제는 다양한 난이도 포함\n" +
                        "- 정답은 0~3 인덱스로 표시\n" +
                        "- JSON 형식으로만 제공\n" +
                        "{\n\"questions\": [{\"question\": \"문제 내용\", \"answers\": [\"1\",\"2\",\"3\",\"4\"], \"correctAnswerIndex\":0}]\n}";

        ChatGPTRequest request = new ChatGPTRequest
        {
            messages = new Message[] { new Message { role = "user", content = prompt } }
        };

        string jsonRequest = JsonConvert.SerializeObject(request);

        using (UnityWebRequest webRequest = new UnityWebRequest(API_URL, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonRequest);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Authorization", $"Bearer {apiKey}");
            webRequest.timeout = 15;

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    string rawResponse = webRequest.downloadHandler.text;
                    Debug.Log("Raw response: " + rawResponse);

                    ChatGPTResponse response = JsonConvert.DeserializeObject<ChatGPTResponse>(rawResponse);

                    if (response?.choices == null || response.choices.Length == 0)
                    {
                        Debug.LogError("ChatGPT 응답이 없습니다.");
                        yield break;
                    }

                    string content = response.choices[0].message.content.Trim();

                    // ``` 제거
                    if (content.StartsWith("```json")) content = content.Substring(7);
                    if (content.StartsWith("```")) content = content.Substring(3);
                    if (content.EndsWith("```")) content = content.Substring(0, content.Length - 3);
                    content = content.Trim();

                    QuizData quizData = JsonConvert.DeserializeObject<QuizData>(content);
                    if (quizData == null || quizData.questions == null || quizData.questions.Length == 0)
                    {
                        Debug.LogError("QuizData 파싱 실패");
                        yield break;
                    }

                    List<QuestionSO> questionSOs = new List<QuestionSO>();
                    foreach (var q in quizData.questions)
                    {
                        QuestionSO so = ScriptableObject.CreateInstance<QuestionSO>();
                        so.SetData(q.question, q.answers, q.correctAnswerIndex);
                        questionSOs.Add(so);
                    }

                    quizGenerateHandler?.Invoke(questionSOs);
                }
                catch (Exception e)
                {
                    Debug.LogError("응답 파싱 오류: " + e.Message);
                }
            }
            else
            {
                Debug.LogError($"API 요청 실패: {webRequest.error} (Code: {webRequest.responseCode})");
                Debug.LogError("응답: " + webRequest.downloadHandler.text);
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
