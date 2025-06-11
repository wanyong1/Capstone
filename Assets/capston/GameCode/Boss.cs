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

        // 싱글 모드에서만 NavMesh 추적
        if (!GameModeManager.IsMultiplayer)
        {
            agent = GetComponent<NavMeshAgent>();
            FindClosestPlayer();
        }
    }

    void Update()
    {
        if (GameModeManager.IsMultiplayer || target == null || agent == null) return;

        // 항상 플레이어 추적
        agent.SetDestination(target.position);
    }

    void FindClosestPlayer()
    {
        float minDist = Mathf.Infinity;
        Player closest = null;

        Player[] allPlayers = FindObjectsByType<Player>(FindObjectsSortMode.None);
        foreach (var player in allPlayers)
        {
            float dist = Vector3.Distance(transform.position, player.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = player;
            }
        }

        if (closest != null)
        {
            target = closest.transform;
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
