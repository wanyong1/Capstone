using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinBonusUpgrade : MonoBehaviour
{
    public Image iconImage;
    public Image[] levelBoxes;
    public TMP_Text costText;

    public int baseUpgradeCost = 100;
    public int upgradeCost;
    public int currentLevel = 0;
    public int maxLevel = 6;

    public Color filledColor = Color.yellow;
    public Color emptyColor = Color.gray;

    public ShopManager shopManager;

    [Header("초기화 설정")]
    [SerializeField] private bool resetOnStart = true;

    void Start()
    {
        if (resetOnStart)
        {
            PlayerPrefs.DeleteKey("CoinBonusUpgradeLevel");
        }

        currentLevel = PlayerPrefs.GetInt("CoinBonusUpgradeLevel", 0);
        upgradeCost = baseUpgradeCost + baseUpgradeCost * currentLevel;
        costText.text = currentLevel >= maxLevel ? "MAX" : upgradeCost.ToString();
        UpdateUI();
    }

    public void OnClickUpgrade()
    {
        if (currentLevel >= maxLevel) return;

        if (shopManager.TrySpendCoins(upgradeCost))
        {
            currentLevel++;
            PlayerPrefs.SetInt("CoinBonusUpgradeLevel", currentLevel);

            upgradeCost += baseUpgradeCost;
            costText.text = currentLevel >= maxLevel ? "MAX" : upgradeCost.ToString();
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        for (int i = 0; i < levelBoxes.Length; i++)
        {
            levelBoxes[i].color = i < currentLevel ? filledColor : emptyColor;
        }

        if (currentLevel >= maxLevel)
        {
            costText.text = "MAX";
        }
    }
}
