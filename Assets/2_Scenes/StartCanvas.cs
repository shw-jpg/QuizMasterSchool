using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartCanvas : MonoBehaviour
{
    [SerializeField] private GameObject startCanvas;
    [SerializeField] private Button[] categoryButtons; // 버튼 배열 (Inspector에서 연결)

    private string[] categories = { "과학", "역사", "스포츠", "영화", "음악" };

    public GameObject lobbyCanvas;
    public Quiz quizManager;

    void Start()
    {
        // 버튼 텍스트를 주제 이름으로 변경
        for (int i = 0; i < categoryButtons.Length; i++)
        {
            TextMeshProUGUI textComp = categoryButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            if (textComp != null)
                textComp.text = categories[i];

            int index = i; // 캡처 문제 방지
            categoryButtons[i].onClick.AddListener(() => OnCategorySelected(index));
        }
    }

    // 카테고리 선택 시 StartCanvas 비활성화 + 퀴즈 시작
    public void OnCategorySelected(int selectedCategory)
    {
        if (startCanvas != null)
            startCanvas.SetActive(false);

        if (lobbyCanvas != null)
            lobbyCanvas.SetActive(false);

        if (quizManager != null)
        {
            quizManager.StartQuiz(selectedCategory);
        }
        else
        {
            Debug.LogError("QuizManager가 StartCanvas에 연결되지 않았습니다!");
        }
    }

    public static class QuizCategory
    {
        public static int selectedCategory = 0;
    }
}
