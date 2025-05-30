using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviourPun
{
    public int maxHealth = 100;
    public int currentHealth;

    private Image healthBarFill;
    private TMP_Text healthBarText;
    private TMP_Text healthBarText_Other;

    private PlayerHealth opponentHealth;

    void Start()
    {
        currentHealth = maxHealth;

        if (!GameModeManager.IsMultiplayer)
        {
            // �̱� ��� UI ����
            healthBarFill = GameObject.Find("HealthBarFill")?.GetComponent<Image>();
            healthBarText = GameObject.Find("HealthBarText")?.GetComponent<TMP_Text>();
            UpdateHealthUI();
            return;
        }

        if (photonView.IsMine)
        {
            Debug.Log("[Start] �� Player ���� (��Ƽ)");

            // �� ü�� UI ����
            healthBarFill = GameObject.Find("HealthBarFill")?.GetComponent<Image>();
            healthBarText = GameObject.Find("HealthBarText")?.GetComponent<TMP_Text>();

            // ��� ü�� ǥ�ÿ� UI ����
            healthBarText_Other = GameObject.Find("HealthBarText_Other")?.GetComponent<TMP_Text>();
            if (healthBarText_Other == null)
                Debug.LogWarning("[UI] HealthBarText_Other ���� ����");

            // ���� ã�� �õ�
            StartCoroutine(FindOpponentHealthWhenReady());
            UpdateHealthUI();
        }
        else
        {
            Debug.Log("[Start] ���� Player �� ���� ��Ȱ��ȭ");
            enabled = false;
        }
    }

    IEnumerator FindOpponentHealthWhenReady()
    {
        while (opponentHealth == null)
        {
            PlayerHealth[] all = FindObjectsByType<PlayerHealth>(FindObjectsSortMode.None);
            foreach (var ph in all)
            {
                if (ph != this && !ph.photonView.IsMine)
                {
                    opponentHealth = ph;
                    Debug.Log("[��� ã��] opponentHealth ���� �Ϸ�");
                    break;
                }
            }

            if (opponentHealth == null)
            {
                Debug.LogWarning("[��� ��] ���� ���� ����... ��õ�");
                yield return new WaitForSeconds(0.5f);
            }
        }

        InvokeRepeating(nameof(UpdateOpponentUI), 0f, 0.2f);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }

        UpdateHealthUI();
    }


    void UpdateHealthUI()
    {
        if (healthBarFill != null)
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;

        if (healthBarText != null)
            healthBarText.text = $"{currentHealth} / {maxHealth}";
    }

    void UpdateOpponentUI()
    {
        if (opponentHealth != null && healthBarText_Other != null)
        {
            healthBarText_Other.text = $"Opponent: {opponentHealth.currentHealth} / {opponentHealth.maxHealth}";
        }
    }

    void Die()
    {
        Debug.Log("�÷��̾� ���");

        if (!GameModeManager.IsMultiplayer)
        {
            // �̱� ����� ��� �ٷ� �й� ó��
            GameOverUIManager.Instance?.ShowGameOver(false, false);
        }
        else
        {
            if (photonView.IsMine)
            {
                Debug.Log("[Die] �� ȭ�� �� LOSE");
                GameOverUIManager.Instance?.ShowGameOver(false, true);

                Debug.Log("[Die] ���濡�� WIN �˸� ����");
                photonView.RPC("NotifyOpponentWin", RpcTarget.OthersBuffered); // �ٽ�
            }
            else
            {
                Debug.Log("[Die] ���� ��ü���� �� ������ ������ (����)");
            }
        }

        gameObject.SetActive(false);
    }

    [PunRPC]
    public void NotifyOpponentWin()
    {
        Debug.Log("[RPC] NotifyOpponentWin ȣ��� �� WIN ó�� �õ�");

        if (GameOverUIManager.Instance != null)
        {
            GameOverUIManager.Instance.ShowGameOver(true, true);
        }
        else
        {
            Debug.LogWarning("[RPC] GameOverUIManager.Instance�� null�Դϴ�");
        }
    }
}
