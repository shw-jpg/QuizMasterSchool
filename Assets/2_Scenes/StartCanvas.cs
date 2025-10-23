using UnityEngine;
using UnityEngine.UI;

public class StartCanvas : MonoBehaviour
{
    [SerializeField] private Button[] categoryButtons;

    void Start()
    {
        for (int i = 0; i < categoryButtons.Length; i++)
        {
            int index = i;
            categoryButtons[i].onClick.AddListener(() =>
            {
                QuizCategory.selectedCategory = index;
                GameManager.Instance.ShowQuiz(); // 퀴즈 화면으로 이동
            });
        }
    }

    public static class QuizCategory
    {
        public static int selectedCategory = 0;
    }
}
