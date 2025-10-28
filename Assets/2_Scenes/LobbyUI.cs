using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private GameObject lobbyCanvas;   // 로비 캔버스
    [SerializeField] private GameObject startCanvas;   // 스타트 캔버스
    [SerializeField] private Button startButton;       // 시작 버튼

    void Start()
    {
        // 시작 버튼 클릭 시 ShowStartCanvas 실행
        startButton.onClick.AddListener(ShowStartCanvas);
    }

    private void ShowStartCanvas()
    {
        if (lobbyCanvas != null)
            lobbyCanvas.SetActive(false);

        if (startCanvas != null)
            startCanvas.SetActive(true);

        Debug.Log("로비 종료 → Start Canvas 활성화");
    }
}
