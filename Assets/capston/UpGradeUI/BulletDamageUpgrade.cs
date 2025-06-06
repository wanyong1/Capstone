using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BulletDamageUpgrade : MonoBehaviour
{
    public Image iconImage;
    public Image[] levelBoxes;
    public TMP_Text costText;

    public int baseUpgradeCost = 500;
    private int upgradeCost;
    private int currentLevel = 0;
    private int maxLevel = 6;

    public Color filledColor = Color.red;
    public Color emptyColor = Color.gray;

    public ShopManager shopManager;

    [Header("초기화 설정")]
    [SerializeField] private bool resetOnStart = true; // 시작 시 초기화 여부

    void Start()
    {
        if (resetOnStart)
        {
            PlayerPrefs.DeleteKey("BulletDamageUpgradeLevel");
        }

        currentLevel = PlayerPrefs.GetInt("BulletDamageUpgradeLevel", 0);
        upgradeCost = baseUpgradeCost + baseUpgradeCost * currentLevel;
        UpdateUI();
    }

    public void OnClickUpgrade()
    {
        if (currentLevel >= maxLevel) return;

        if (shopManager.TrySpendCoins(upgradeCost))
        {
            currentLevel++;
            PlayerPrefs.SetInt("BulletDamageUpgradeLevel", currentLevel);

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

        costText.text = currentLevel >= maxLevel ? ": MAX" : $": {upgradeCost}";
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }
}
