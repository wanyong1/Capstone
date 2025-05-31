using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum UpgradeType
{
    BulletDamage,
    MoveSpeed,
    ExpBonus,
    CoinBonus,
    MaxHealth
}

public class UpgradeSlot : MonoBehaviour
{
    public Image iconImage;
    public Image[] levelBoxes;
    public TMP_Text costText;

    public int baseUpgradeCost = 1;
    public int upgradeCost;
    public int currentLevel = 0;
    public int maxLevel = 5;

    public Color filledColor = new Color(0, 0.8f, 1f); // �Ķ���
    public Color emptyColor = new Color(0.3f, 0.3f, 0.3f); // ȸ��

    public ShopManager shopManager;
    public UpgradeType upgradeType;

    void Start()
    {
        upgradeCost = baseUpgradeCost;
        costText.text = upgradeCost.ToString();
        UpdateLevelUI();
    }

    public void OnClickUpgrade()
    {
        if (currentLevel >= maxLevel) return;

        if (shopManager.TrySpendCoins(upgradeCost))
        {
            currentLevel++;
            upgradeCost += baseUpgradeCost; // ���� ��� ����
            costText.text = upgradeCost.ToString();
            UpdateLevelUI();
            ApplyUpgradeEffect();
        }
    }

    void UpdateLevelUI()
    {
        for (int i = 0; i < levelBoxes.Length; i++)
        {
            levelBoxes[i].color = i < currentLevel ? filledColor : emptyColor;
        }
    }

    void ApplyUpgradeEffect()
    {
        switch (upgradeType)
        {
            case UpgradeType.BulletDamage:
                PlayerPrefs.SetInt("BulletDamageUpgradeLevel", currentLevel);
                break;
            case UpgradeType.MoveSpeed:
                PlayerPrefs.SetInt("MoveSpeedUpgradeLevel", currentLevel);
                break;
            case UpgradeType.ExpBonus:
                PlayerPrefs.SetInt("ExpBonusUpgradeLevel", currentLevel);
                break;
            case UpgradeType.MaxHealth:
                PlayerPrefs.SetInt("MaxHealthUpgradeLevel", currentLevel);
                break;
                // case UpgradeType.CoinBonus: �������� ����
        }
    }

}
