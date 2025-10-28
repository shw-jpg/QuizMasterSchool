using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppleSlotDisplay : MonoBehaviour
{
    [SerializeField] GameObject applePrefab;   // 사과 프리팹
    [SerializeField] Transform[] slots;        // 슬롯 5개 (사각형 영역 내부)

    private List<GameObject> filledApples = new List<GameObject>();

    void Start()
    {
        // 시작 시 5개 사과 생성
        ResetApples();
        CreateAllApples();
    }

    // 처음부터 5개 채우기
    private void CreateAllApples()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            Transform slot = slots[i];
            GameObject apple = Instantiate(applePrefab, slot);

            RectTransform rt = apple.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0.5f, 0.5f);
                rt.anchoredPosition = Vector2.zero;
                rt.localScale = Vector3.one;
            }

            filledApples.Add(apple);
        }
    }

    // 정답 → 사과 밝게(초기 상태 유지)
    public void AddApple()
    {
        foreach (var apple in filledApples)
        {
            Image img = apple.GetComponent<Image>();
            if (img != null)
                img.color = Color.white;
        }
    }

    // 오답 → 사과 어둡게
    public void RemoveApple()
    {
        foreach (var apple in filledApples)
        {
            Image img = apple.GetComponent<Image>();
            if (img != null)
            {
                // 점점 어둡게 (밝기 70%)
                img.color = new Color(0.3f, 0.3f, 0.3f);
            }
        }
    }

    // 전체 리셋
    public void ResetApples()
    {
        foreach (GameObject apple in filledApples)
            Destroy(apple);

        filledApples.Clear();
    }
}
