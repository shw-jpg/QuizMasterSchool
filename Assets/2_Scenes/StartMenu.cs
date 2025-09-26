using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private GameObject startCanvas;
    [SerializeField] private Button[] categoryButtons; // 버튼 배열 (Inspector에 연결)

    private string[] topics = { "과학", "역사", "스포츠", "영화", "음악" };

    void Start()
    {
        // 실행할 때 버튼 텍스트를 주제 이름으로 변경
        for (int i = 0; i < categoryButtons.Length && i < topics.Length; i++)
        {
            TextMeshProUGUI textComp = categoryButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            if (textComp != null)
                textComp.text = topics[i];

            int index = i; // 캡처 문제 방지
            categoryButtons[i].onClick.AddListener(() => SelectCategory(index));
        }
    }

    public void SelectCategory(int categoryIndex)
    {
        QuizCategory.selectedCategory = categoryIndex;

        if (startCanvas != null)
            startCanvas.SetActive(false);

        SceneManager.LoadScene("QuizScene");
    }

    public static class QuizCategory
    {
        public static int selectedCategory = 0;
    }
}
