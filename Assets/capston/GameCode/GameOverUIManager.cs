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
    public Button restartButton; //  ����� ��ư ����

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject); // �� ��ȯ �� ����
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
            gameOverPanel.SetActive(false); // ���� ���� �� ������ ���α�
        }
    }

    public void ShowGameOver(bool isWin, bool isMultiplayer)
    {
        Debug.Log($"[GameOverUI] ShowGameOver ȣ��� - ���: {(isWin ? "WIN" : "LOSE")}, ���: {(isMultiplayer ? "��Ƽ" : "�̱�")}");

        if (gameOverPanel != null)
        {
            Debug.Log("[GameOverUI] gameOverPanel Ȱ��ȭ �õ�");
            gameOverPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("[GameOverUI] gameOverPanel �� null�Դϴ�!");
        }

        if (resultText != null)
        {
            if (isMultiplayer)
            {
                resultText.text = isWin ? "WIN" : "LOSE";
            }
            else
            {
                resultText.text = ""; // �̱ۿ����� ��� �ؽ�Ʈ ����
            }
        }
        else
        {
            Debug.LogWarning("[GameOverUI] resultText �� null�Դϴ�!");
        }

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Debug.Log("[GameOverUI] RestartGame ȣ���");

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameOverPanel.SetActive(false); // ����� �� UI �����
    }

    public void ReturnToMenu()
    {
        Debug.Log("[GameOverUI] ReturnToMenu ȣ���");

        Time.timeScale = 1f;
        SceneManager.LoadScene("UI"); // �޴� �� �̸��� �°� ���� �ʿ�
        gameOverPanel.SetActive(false);
    }
}
