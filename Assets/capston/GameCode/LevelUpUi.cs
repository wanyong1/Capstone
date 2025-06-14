using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;

public class LevelUpUI : MonoBehaviour
{
    public GameObject panel;
    public Button[] upgradeButtons;
    public Player player; // 동적으로 연결됨

    void Start()
    {
        if (player == null)
        {
            var allPlayers = FindObjectsByType<Player>(FindObjectsSortMode.None);
            foreach (var p in allPlayers)
            {
                var view = p.GetComponent<PhotonView>();
                if (view == null || view.IsMine)
                {
                    player = p;
                    break;
                }
            }
        }
    }

    public void Show()
    {
        if (!GameModeManager.IsMultiplayer)
        {
            Time.timeScale = 0f;
        }

        panel.SetActive(true);
        gameObject.SetActive(true);

        List<string> allUpgrades = new List<string> { "Orbit", "Damage", "FireRate", "addBullet" };
        var options = allUpgrades.OrderBy(x => Random.value).Take(3).ToList();

        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            if (i < options.Count)
            {
                string upgradeType = options[i];
                upgradeButtons[i].gameObject.SetActive(true);
                upgradeButtons[i].GetComponentInChildren<Text>().text = upgradeType;
                upgradeButtons[i].onClick.RemoveAllListeners();
                upgradeButtons[i].onClick.AddListener(() => SelectUpgrade(upgradeType));
            }
            else
            {
                upgradeButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void SelectUpgrade(string type)
    {
        if (player == null) return;

        switch (type)
        {
            case "Orbit":
                player.AddOrbitingObject();
                break;
            case "Damage":
                player.plusBulletDamage += 2;
                break;
            case "FireRate":
                player.fireRate = Mathf.Max(0.1f, player.fireRate - 0.2f);
                break;
            case "addBullet":
                player.plusBulletCount += 1;
                break;
        }

        InGameUpgradeUIManager.Instance?.AddUpgrade(type);

        if (!GameModeManager.IsMultiplayer)
        {
            Time.timeScale = 1f;
        }

        panel.SetActive(false);
        gameObject.SetActive(false);
    }
}
