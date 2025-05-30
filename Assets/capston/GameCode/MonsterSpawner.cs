using UnityEngine;
using Photon.Pun;
using System.Collections;

public class MonsterSpawner : MonoBehaviourPun
{
    [Header("싱글 전용 프리팹 (Drag & Drop)")]
    public GameObject[] prefabListSingleMode;

    [Header("멀티 전용 프리팹 이름 (Resources 폴더에 존재해야 함)")]
    public string[] monsterPrefabNames;

    public float spawnInterval = 5f;
    public int monstersPerSpawn = 5;

    [Header("멀티용 스폰 포인트")]
    public Transform[] incomingSpawnPoints_Host;
    public Transform[] incomingSpawnPoints_Client;

    void Start()
    {
        if (GameModeManager.IsMultiplayer)
        {
            StartCoroutine(SpawnEnemiesMulti());
        }
        else
        {
            StartCoroutine(SpawnEnemiesSingle());
        }
    }

    // 싱글모드 몬스터 스폰 (Drag & Drop 방식)
    IEnumerator SpawnEnemiesSingle()
    {
        while (true)
        {
            for (int i = 0; i < monstersPerSpawn; i++)
            {
                int index = Random.Range(0, prefabListSingleMode.Length);
                GameObject prefab = prefabListSingleMode[index];

                if (prefab == null)
                {
                    Debug.LogWarning($"[싱글] 인스펙터에 프리팹이 연결되지 않았습니다 (index: {index})");
                    continue;
                }

                Vector3 spawnPos = GetRandomSpawnPositionSingle();
                Instantiate(prefab, spawnPos, Quaternion.identity);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // 멀티모드 몬스터 스폰
    IEnumerator SpawnEnemiesMulti()
    {
        while (true)
        {
            for (int i = 0; i < monstersPerSpawn; i++)
            {
                string prefabName = monsterPrefabNames[Random.Range(0, monsterPrefabNames.Length)];
                Vector3 spawnPos = GetRandomSpawnPositionMulti();
                PhotonNetwork.Instantiate(prefabName, spawnPos, Quaternion.identity);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    Vector3 GetRandomSpawnPositionSingle()
    {
        Vector3 center = new Vector3(0f, 0f, 0f);
        Vector3 offset = Quaternion.Euler(0, 45, 0) * new Vector3(Random.Range(-250, 250), 2f, Random.Range(-250, 250));
        return center + offset;
    }

    Vector3 GetRandomSpawnPositionMulti()
    {
        Vector3 center = PhotonNetwork.IsMasterClient ? new Vector3(0f, 0f, 0f) : new Vector3(850f, 0f, 850f);
        Vector3 offset = Quaternion.Euler(0, 45, 0) * new Vector3(Random.Range(-250, 250), 2f, Random.Range(-250, 250));
        return center + offset;
    }

    [PunRPC]
    public void SpawnEnemyFromOtherPlayer(string prefabName)
    {
        Debug.Log($"[RPC] SpawnEnemyFromOtherPlayer 호출됨 - {prefabName}");

        Transform[] targetPoints = PhotonNetwork.IsMasterClient ? incomingSpawnPoints_Host : incomingSpawnPoints_Client;

        if (targetPoints == null || targetPoints.Length == 0)
        {
            Debug.LogWarning("[Spawner] 스폰 포인트가 없습니다.");
            return;
        }

        int index = Random.Range(0, targetPoints.Length);
        Vector3 spawnPos = targetPoints[index].position;

        GameObject enemy = PhotonNetwork.Instantiate(prefabName, spawnPos, Quaternion.identity);

        PhotonView pv = enemy.GetComponent<PhotonView>();
        if (pv != null)
        {
            pv.RPC("SetAsFromOtherPlayer", RpcTarget.AllBuffered);
        }
    }
}
