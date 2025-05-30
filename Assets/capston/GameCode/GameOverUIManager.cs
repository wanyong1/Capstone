using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverUIManager : MonoBehaviour
{
    public static GameOverUIManager Instance;

    [Header("UI References")]
    public GameObject gameOverPanel;
    public TMP_Text resultText;
    public Button returnButton;
    public Button restartButton; //  재시작 버튼 연결

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject); // 씬 전환 시 유지
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false); // 게임 시작 시 무조건 꺼두기
        }
    }

    public void ShowGameOver(bool isWin, bool isMultiplayer)
    {
        Debug.Log($"[GameOverUI] ShowGameOver 호출됨 - 결과: {(isWin ? "WIN" : "LOSE")}, 모드: {(isMultiplayer ? "멀티" : "싱글")}");

        if (gameOverPanel != null)
        {
            Debug.Log("[GameOverUI] gameOverPanel 활성화 시도");
            gameOverPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("[GameOverUI] gameOverPanel 이 null입니다!");
        }

        if (resultText != null)
        {
            if (isMultiplayer)
            {
                resultText.text = isWin ? "WIN" : "LOSE";
            }
            else
            {
                resultText.text = ""; // 싱글에서는 결과 텍스트 숨김
            }
        }
        else
        {
            Debug.LogWarning("[GameOverUI] resultText 가 null입니다!");
        }

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Debug.Log("[GameOverUI] RestartGame 호출됨");

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameOverPanel.SetActive(false); // 재시작 후 UI 숨기기
    }

    public void ReturnToMenu()
    {
        Debug.Log("[GameOverUI] ReturnToMenu 호출됨");

        Time.timeScale = 1f;
        SceneManager.LoadScene("UI"); // 메뉴 씬 이름에 맞게 수정 필요
        gameOverPanel.SetActive(false);
    }
}
