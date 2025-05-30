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
            // 싱글 모드 UI 연결
            healthBarFill = GameObject.Find("HealthBarFill")?.GetComponent<Image>();
            healthBarText = GameObject.Find("HealthBarText")?.GetComponent<TMP_Text>();
            UpdateHealthUI();
            return;
        }

        if (photonView.IsMine)
        {
            Debug.Log("[Start] 내 Player 시작 (멀티)");

            // 내 체력 UI 연결
            healthBarFill = GameObject.Find("HealthBarFill")?.GetComponent<Image>();
            healthBarText = GameObject.Find("HealthBarText")?.GetComponent<TMP_Text>();

            // 상대 체력 표시용 UI 연결
            healthBarText_Other = GameObject.Find("HealthBarText_Other")?.GetComponent<TMP_Text>();
            if (healthBarText_Other == null)
                Debug.LogWarning("[UI] HealthBarText_Other 연결 실패");

            // 상대방 찾기 시도
            StartCoroutine(FindOpponentHealthWhenReady());
            UpdateHealthUI();
        }
        else
        {
            Debug.Log("[Start] 상대방 Player → 로직 비활성화");
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
                    Debug.Log("[상대 찾음] opponentHealth 연결 완료");
                    break;
                }
            }

            if (opponentHealth == null)
            {
                Debug.LogWarning("[대기 중] 아직 상대방 없음... 재시도");
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
        Debug.Log("플레이어 사망");

        if (!GameModeManager.IsMultiplayer)
        {
            // 싱글 모드일 경우 바로 패배 처리
            GameOverUIManager.Instance?.ShowGameOver(false, false);
        }
        else
        {
            if (photonView.IsMine)
            {
                Debug.Log("[Die] 내 화면 → LOSE");
                GameOverUIManager.Instance?.ShowGameOver(false, true);

                Debug.Log("[Die] 상대방에게 WIN 알림 전송");
                photonView.RPC("NotifyOpponentWin", RpcTarget.OthersBuffered); // 핵심
            }
            else
            {
                Debug.Log("[Die] 상대방 객체지만 내 로직에 도달함 (무시)");
            }
        }

        gameObject.SetActive(false);
    }

    [PunRPC]
    public void NotifyOpponentWin()
    {
        Debug.Log("[RPC] NotifyOpponentWin 호출됨 → WIN 처리 시도");

        if (GameOverUIManager.Instance != null)
        {
            GameOverUIManager.Instance.ShowGameOver(true, true);
        }
        else
        {
            Debug.LogWarning("[RPC] GameOverUIManager.Instance가 null입니다");
        }
    }
}
