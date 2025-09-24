using System.Collections.Generic;
using UnityEngine;

public class AppleSlotDisplay : MonoBehaviour
{
    [SerializeField] GameObject applePrefab;   // 사과 아이콘 프리팹 (사과.png)
    [SerializeField] Transform[] slots;        // 슬롯 위치 5개 (Inspector에서 채워줌)

    private List<GameObject> filledApples = new List<GameObject>();

    // 정답 맞췄을 때 → 사과 추가
    public void AddApple()
    {
        if (filledApples.Count < slots.Length)
        {
            Transform slot = slots[filledApples.Count];
            GameObject apple = Instantiate(applePrefab, slot); // slot을 부모로 생성

            // 위치/크기 초기화
            RectTransform rt = apple.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.anchorMin = new Vector2(0.5f, 0.5f);
                rt.anchorMax = new Vector2(0.5f, 0.5f);
                rt.pivot = new Vector2(0.5f, 0.5f);

                rt.anchoredPosition = Vector2.zero;   // 슬롯 중앙에 위치
                rt.localScale = Vector3.one;          // 크기 초기화
            }

            filledApples.Add(apple);
        }
    }

    // 오답 → 마지막 사과 제거
    public void RemoveApple()
    {
        if (filledApples.Count > 0)
        {
            GameObject lastApple = filledApples[filledApples.Count - 1];
            filledApples.RemoveAt(filledApples.Count - 1);
            Destroy(lastApple);
        }
    }

    // 게임 다시 시작 시 초기화
    public void ResetApples()
    {
        foreach (GameObject apple in filledApples)
        {
            Destroy(apple);
        }
        filledApples.Clear();
    }
}
