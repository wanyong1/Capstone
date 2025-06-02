using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public int playerCoins = 0;
    public TMP_Text coinText;

    int bulletDamageLevel = 0;

    void Start()
    {
        playerCoins = PlayerPrefs.GetInt("TotalCoins", 0);
        UpdateCoinUI();
    }

    public bool TrySpendCoins(int amount)
    {
        if (playerCoins >= amount)
        {
            playerCoins -= amount;
            PlayerPrefs.SetInt("TotalCoins", playerCoins);
            UpdateCoinUI();
            return true;
        }
        return false;
    }

    void UpdateCoinUI()
    {
        coinText.text = "Coins: " + playerCoins;
    }

    public void UpgradeBulletDamage()
    {
        bulletDamageLevel++;
        Debug.Log("Bullet damage upgraded! Level: " + bulletDamageLevel);
    }
}
