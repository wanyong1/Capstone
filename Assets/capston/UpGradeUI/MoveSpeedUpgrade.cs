using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoveSpeedUpgrade : MonoBehaviour
{
    public Image iconImage;
    public Image[] levelBoxes;
    public TMP_Text costText;

    public int baseUpgradeCost = 600;
    private int upgradeCost;
    private int currentLevel = 0;
    private int maxLevel = 10;

    public Color filledColor = new Color(0.3f, 0.7f, 1f); // ¹àÀº ÆÄ¶û
    public Color emptyColor = Color.gray;

    public ShopManager shopManager;

    [Header("ÃÊ±âÈ­ ¿É¼Ç")]
    [SerializeField] private bool resetOnStart = true;

    void Start()
    {
        if (resetOnStart)
        {
            PlayerPrefs.DeleteKey("MoveSpeedUpgradeLevel");
        }

        currentLevel = PlayerPrefs.GetInt("MoveSpeedUpgradeLevel", 0);
        upgradeCost = baseUpgradeCost + baseUpgradeCost * currentLevel;
        UpdateUI();
    }

    public void OnClickUpgrade()
    {
        if (currentLevel >= maxLevel) return;

        if (shopManager.TrySpendCoins(upgradeCost))
        {
            currentLevel++;
            PlayerPrefs.SetInt("MoveSpeedUpgradeLevel", currentLevel);

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

        costText.text = currentLevel >= maxLevel ? "MAX" : $"{upgradeCost}";
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }
}
