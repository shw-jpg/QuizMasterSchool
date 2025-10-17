using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartCanvas : MonoBehaviour
{
    [SerializeField] private GameObject startCanvas;
    [SerializeField] private Button[] categoryButtons;

    // 이동할 씬 이름을 하드코딩
    private string targetSceneName = "QuizScene";

    private string[] topics = { "과학", "역사", "스포츠", "영화", "음악" };

    void Start()
    {
        for (int i = 0; i < categoryButtons.Length && i < topics.Length; i++)
        {
            TextMeshProUGUI textComp = categoryButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            if (textComp != null) textComp.text = topics[i];

            int index = i;
            categoryButtons[i].onClick.AddListener(() => SelectCategory(index));
        }
    }

    public void SelectCategory(int categoryIndex)
    {
        QuizCategory.selectedCategory = categoryIndex;

        if (startCanvas != null)
            startCanvas.SetActive(false);

        // Build Profiles 없이 씬 전환
        SceneManager.LoadScene(targetSceneName);
    }

    [System.Serializable]
    public static class QuizCategory
    {
        public static int selectedCategory = 0;
    }
}
