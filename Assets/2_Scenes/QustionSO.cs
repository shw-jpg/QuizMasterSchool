using UnityEngine;

[CreateAssetMenu(menuName = "Quiz Question", fileName = "Nwq Auestion")]
public class QuestionSO : ScriptableObject
{
    [TextArea(2, 6)]
    [SerializeField] string question = "���⿡ ������ �����ּ���";
}
