using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public float maxHealth = 1000f;
    private float currentHealth;
    private bool isDying = false;

    public GameObject expOrbPrefab;
    public GameObject coinPrefab;
    public int expAmount = 10;     // 드랍할 경험치 오브 갯수
    public int coinAmount = 5;     // 드랍할 코인 갯수

    void Start()
    {
        currentHealth = maxHealth;

        BossUIManager.Instance?.Show(maxHealth);
    }

    public void TakeDamage(float amount)
    {
        if (isDying) return;

        currentHealth -= amount;
        currentHealth = Mathf.Max(0, currentHealth);

        Debug.Log($"[Boss] {gameObject.name} 남은 체력: {currentHealth}");

        BossUIManager.Instance?.UpdateHP(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            isDying = true;
            Die();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
                playerHealth.TakeDamage(10);
        }
    }

    void Die()
    {
        Debug.Log("[Boss] 보스 사망");

        // 1. UI 숨기기
        BossUIManager.Instance?.Hide();

        // 2. 경험치 오브 대량 드랍 (싱글모드만)
        if (!GameModeManager.IsMultiplayer && expOrbPrefab != null)
        {
            for (int i = 0; i < expAmount; i++)
            {
                Vector3 offset = Random.insideUnitSphere * 2f;
                offset.y = 0.5f;
                Instantiate(expOrbPrefab, transform.position + offset, Quaternion.identity);
            }
        }

        // 3. 코인 대량 드랍 (싱글모드만)
        if (!GameModeManager.IsMultiplayer && coinPrefab != null)
        {
            for (int i = 0; i < coinAmount; i++)
            {
                Vector3 offset = Random.insideUnitSphere * 2f;
                offset.y = 0.5f;
                Instantiate(coinPrefab, transform.position + offset, Quaternion.identity);
            }
        }

        // 4. 오브젝트 제거
        Destroy(gameObject);
    }

}
