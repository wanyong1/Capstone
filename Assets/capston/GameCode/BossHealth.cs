using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public float maxHealth = 1000f;
    private float currentHealth;
    private bool isDying = false;

    public GameObject expOrbPrefab;
    public GameObject coinPrefab;
    public int expAmount = 10;     // ����� ����ġ ���� ����
    public int coinAmount = 5;     // ����� ���� ����

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

        Debug.Log($"[Boss] {gameObject.name} ���� ü��: {currentHealth}");

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
        Debug.Log("[Boss] ���� ���");

        // 1. UI �����
        BossUIManager.Instance?.Hide();

        // 2. ����ġ ���� �뷮 ��� (�̱۸�常)
        if (!GameModeManager.IsMultiplayer && expOrbPrefab != null)
        {
            for (int i = 0; i < expAmount; i++)
            {
                Vector3 offset = Random.insideUnitSphere * 2f;
                offset.y = 0.5f;
                Instantiate(expOrbPrefab, transform.position + offset, Quaternion.identity);
            }
        }

        // 3. ���� �뷮 ��� (�̱۸�常)
        if (!GameModeManager.IsMultiplayer && coinPrefab != null)
        {
            for (int i = 0; i < coinAmount; i++)
            {
                Vector3 offset = Random.insideUnitSphere * 2f;
                offset.y = 0.5f;
                Instantiate(coinPrefab, transform.position + offset, Quaternion.identity);
            }
        }

        // 4. ������Ʈ ����
        Destroy(gameObject);
    }

}
