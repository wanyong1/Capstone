using UnityEngine;
using Photon.Pun;
using System.Collections;

public class MonsterSpawner : MonoBehaviourPun
{
    [Header("�̱� ���� ������ (Drag & Drop)")]
    public GameObject[] prefabListSingleMode;

    [Header("��Ƽ ���� ������ �̸� (Resources ������ �����ؾ� ��)")]
    public string[] monsterPrefabNames;

    public float spawnInterval = 5f;
    public int monstersPerSpawn = 5;

    [Header("��Ƽ�� ���� ����Ʈ")]
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

    // �̱۸�� ���� ���� (Drag & Drop ���)
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
                    Debug.LogWarning($"[�̱�] �ν����Ϳ� �������� ������� �ʾҽ��ϴ� (index: {index})");
                    continue;
                }

                Vector3 spawnPos = GetRandomSpawnPositionSingle();
                Instantiate(prefab, spawnPos, Quaternion.identity);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // ��Ƽ��� ���� ����
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
        Debug.Log($"[RPC] SpawnEnemyFromOtherPlayer ȣ��� - {prefabName}");

        Transform[] targetPoints = PhotonNetwork.IsMasterClient ? incomingSpawnPoints_Host : incomingSpawnPoints_Client;

        if (targetPoints == null || targetPoints.Length == 0)
        {
            Debug.LogWarning("[Spawner] ���� ����Ʈ�� �����ϴ�.");
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
