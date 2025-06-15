using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    public float maxHealth = 1000f;
    private float currentHealth;
    private bool isDying = false;

    public GameObject expOrbPrefab;
    public GameObject coinPrefab;
    public int expAmount = 10;
    public int coinAmount = 5;

    private NavMeshAgent agent;
    private Transform target;

    void Start()
    {
        currentHealth = maxHealth;
        BossUIManager.Instance?.Show(maxHealth);

        agent = GetComponent<NavMeshAgent>();
        FindClosestPlayer(); // 추적 대상 설정
    }

    void Update()
    {
        if (target != null && agent != null && !isDying)
        {
            agent.SetDestination(target.position);
        }
    }

    void FindClosestPlayer()
    {
        float minDistance = float.MaxValue;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            float dist = Vector3.Distance(transform.position, player.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                target = player.transform;
            }
        }
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

        BossUIManager.Instance?.Hide();

        if (!GameModeManager.IsMultiplayer && expOrbPrefab != null)
        {
            for (int i = 0; i < expAmount; i++)
            {
                Vector3 offset = Random.insideUnitSphere * 2f;
                offset.y = 0.5f;
                Instantiate(expOrbPrefab, transform.position + offset, Quaternion.identity);
            }
        }

        if (!GameModeManager.IsMultiplayer && coinPrefab != null)
        {
            for (int i = 0; i < coinAmount; i++)
            {
                Vector3 offset = Random.insideUnitSphere * 2f;
                offset.y = 0.5f;
                Instantiate(coinPrefab, transform.position + offset, Quaternion.identity);
            }
        }

        Destroy(gameObject);
    }
}
