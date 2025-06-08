using UnityEngine;
using Photon.Pun;

public class PlayerExp : MonoBehaviour
{
    public int level = 1;
    public int currentExp = 0;
    public int expToNextLevel = 5;

    public LevelUpUI levelUpUI;
    private PhotonView photonView;

    void Start()
    {
        photonView = GetComponent<PhotonView>();

        if (GameModeManager.IsMultiplayer && !photonView.IsMine)
        {
            enabled = false;
            return;
        }

        GameObject uiObj = GameObject.Find("PlayerUpgradeUI");
        if (uiObj != null)
        {
            levelUpUI = uiObj.GetComponent<LevelUpUI>();
            uiObj.SetActive(false); // ó������ ���д�
        }

        if (levelUpUI == null)
        {
            Debug.LogWarning(" PlayerUpgradeUI ���� ����");
        }
    }

    public void AddExp(int amount)
    {
        if (!GameModeManager.IsMultiplayer)
        {
            float multiplier = 1f + (UpgradeStatsManager.Instance?.GetBonusExpAmount() ?? 0) * 0.1f;
            amount = Mathf.RoundToInt(amount * multiplier);
        }
        currentExp += amount;

        if (currentExp >= expToNextLevel)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        currentExp -= expToNextLevel;
        level++;
        expToNextLevel = Mathf.RoundToInt(expToNextLevel * 1.5f);

        levelUpUI?.Show();
    }
}
