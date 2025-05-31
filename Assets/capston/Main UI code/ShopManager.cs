using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public int playerCoins = 500;
    public TMP_Text coinText;

    int bulletDamageLevel = 0;

    void Start()
    {
        PlayerPrefs.DeleteKey("CoinBonusUpgradeLevel");
        UpdateCoinUI();
    }

    public bool TrySpendCoins(int amount)
    {
        if (playerCoins >= amount)
        {
            playerCoins -= amount;
            UpdateCoinUI();
            return true;
        }
        return false;
    }

    void UpdateCoinUI()
    {
        coinText.text = playerCoins.ToString();
    }

    public void UpgradeBulletDamage()
    {
        bulletDamageLevel++;
        Debug.Log("Bullet damage upgraded! Level: " + bulletDamageLevel);
        // 실제 스탯 증가 로직은 여기에 연결
    }

}
