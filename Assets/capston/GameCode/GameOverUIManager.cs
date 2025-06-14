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
                resultText.text = "GameOver"; // �̱ۿ����� ��� �ؽ�Ʈ ����
            }
        }
        else
        {
            Debug.LogWarning("[GameOverUI] resultText �� null�Դϴ�!");
        }

        //  ���� ���� �� ���� UI�� ���� ó��
        if (BossUIManager.Instance != null)
        {
            Debug.Log("[GameOverUI] BossUIManager ���� ó��");
            BossUIManager.Instance.Hide();
        }
        //InGameUpgradeUI ��Ȱ��ȭ
        if (InGameUpgradeUIManager.Instance != null)
        {
            Debug.Log("[GameOverUI] InGameUpgradeUI ���� ó��");
            InGameUpgradeUIManager.Instance.HidePanel();
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

        //  ���� ����
        if (CoinManager.Instance != null)
            CoinManager.Instance.SaveCoinsToPrefs();

        SceneManager.LoadScene("UI"); // �޴� ������
        gameOverPanel.SetActive(false);
    }

}
