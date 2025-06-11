using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public GameObject bossPrefab;
    public float spawnInterval = 300f; // 5분
    private float timer;

    void Update()
    {
        if (GameModeManager.IsMultiplayer) return;

        // 보스가 살아있으면 타이머 멈춤
        if (FindAnyObjectByType<Boss>() != null) return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnBoss();
            timer = 0f;
        }
    }

    void SpawnBoss()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");

        Vector3 spawnPos = transform.position;

        if (playerObj != null)
        {
            Vector3 offset = Random.onUnitSphere;
            offset.y = 0f;
            offset = offset.normalized * 30f;
            spawnPos = playerObj.transform.position + offset;
        }

        Instantiate(bossPrefab, spawnPos, Quaternion.identity);

        Debug.Log("[BossSpawner] 보스가 소환되었습니다.");
    }
}
