using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InGameUpgradeUIManager : MonoBehaviour
{
    public static InGameUpgradeUIManager Instance;
    public GameObject upgradePanel;


    public TMP_Text upgradeText;
    private Dictionary<string, int> upgradeLevels = new();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddUpgrade(string type)
    {
        if (!upgradeLevels.ContainsKey(type))
            upgradeLevels[type] = 1;
        else
            upgradeLevels[type]++;

        UpdateUI();
    }

    private void UpdateUI()
    {
        upgradeText.text = "";
        foreach (var kvp in upgradeLevels)
        {
            upgradeText.text += $"{kvp.Key} LV: {kvp.Value}\n";
        }
    }
    public void HidePanel()
    {
        if (upgradePanel != null)
            upgradePanel.SetActive(false);
    }

}
