using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;
    public TMP_Text coinTextInGame;
    public GameObject coinIconInGame;
    private int currentCoins = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ResetCoinCount();
        UpdateCoinVisibility();
        SceneManager.sceneLoaded += OnSceneLoaded; // 씬 전환 시마다 다시 확인
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateCoinVisibility();

        if (scene.name == "GameScene")
        {
            ResetCoinCount();  // 게임을 다시 시작할 때 코인 초기화
        }
    }

    void UpdateCoinVisibility()
    {
        bool isSingleGameScene = SceneManager.GetActiveScene().name == "GameScene";

        if (coinTextInGame != null)
            coinTextInGame.gameObject.SetActive(isSingleGameScene);

        if (coinIconInGame != null)
            coinIconInGame.SetActive(isSingleGameScene);
    }

    public void AddCoin(int amount)
    {
        currentCoins += amount;
        UpdateCoinUI();
    }

    public int GetCurrentCoins()
    {
        return currentCoins;
    }

    public void ResetCoinCount()
    {
        currentCoins = 0;
        UpdateCoinUI();
    }

    void UpdateCoinUI()
    {
        if (coinTextInGame != null)
            coinTextInGame.text = currentCoins.ToString();
    }

    public void SaveCoinsToPrefs()
    {
        int totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
        PlayerPrefs.SetInt("TotalCoins", totalCoins + currentCoins);
        PlayerPrefs.Save();
    }
}
