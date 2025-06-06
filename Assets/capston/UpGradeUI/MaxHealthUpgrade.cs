using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MaxHealthUpgrade : MonoBehaviour
{
    public Image iconImage;
    public Image[] levelBoxes;
    public TMP_Text costText;

    public int baseUpgradeCost = 600;
    private int upgradeCost;
    private int currentLevel = 0;
    private int maxLevel = 6;

    public Color filledColor = new Color(0.3f, 0.7f, 1f); // 파란색
    public Color emptyColor = Color.gray;

    public ShopManager shopManager;

    [Header("초기화 옵션")]
    [SerializeField] private bool resetOnStart = true;

    void Start()
    {
        if (resetOnStart)
        {
            PlayerPrefs.DeleteKey("MaxHealthUpgradeLevel");
        }

        currentLevel = PlayerPrefs.GetInt("MaxHealthUpgradeLevel", 0);
        upgradeCost = baseUpgradeCost + baseUpgradeCost * currentLevel;
        UpdateUI();
    }

    public void OnClickUpgrade()
    {
        if (currentLevel >= maxLevel) return;

        if (shopManager.TrySpendCoins(upgradeCost))
        {
            currentLevel++;
            PlayerPrefs.SetInt("MaxHealthUpgradeLevel", currentLevel);

            upgradeCost += baseUpgradeCost;
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        for (int i = 0; i < levelBoxes.Length; i++)
        {
            levelBoxes[i].color = i < currentLevel ? filledColor : emptyColor;
        }

        costText.text = currentLevel >= maxLevel ? "" : $"{upgradeCost}";
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }
}
