using UnityEngine;
using Photon.Pun;

public class BossSpawner : MonoBehaviour
{
    [Header("싱글 모드용 보스 프리팹")]
    public GameObject bossPrefabSingle;

    [Header("보스 소환 대기 시간 (초)")]
    public float delay = 300f;

    private float timer = 0f;
    private GameObject spawnedBoss;

    void Update()
    {
        if (spawnedBoss != null) return;

        timer += Time.deltaTime;

        if (timer >= delay)
        {
            SpawnBoss();
            timer = 0f;
        }
    }

    void SpawnBoss()
    {
        // 이미 보스가 있다면 소환하지 않음
        if (GameObject.FindWithTag("boss") != null)
        {
            Debug.Log("[BossSpawner] 보스가 이미 존재하여 소환 생략");
            return;
        }

        GameObject myPlayer = FindMyPlayer();
        if (myPlayer == null)
        {
            Debug.LogWarning("[BossSpawner] 내 플레이어를 찾지 못함");
            return;
        }

        Vector3 offset = Random.onUnitSphere;
        offset.y = 0f;
        offset = offset.normalized * 30f;

        Vector3 spawnPos = myPlayer.transform.position + offset;

        if (GameModeManager.IsMultiplayer)
        {
            spawnedBoss = PhotonNetwork.Instantiate("Boss", spawnPos, Quaternion.identity);
        }
        else
        {
            spawnedBoss = Instantiate(bossPrefabSingle, spawnPos, Quaternion.identity);
        }

        Debug.Log($"[BossSpawner] 보스 소환 완료 @ {spawnPos}");
    }

    GameObject FindMyPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (var player in players)
        {
            PhotonView view = player.GetComponent<PhotonView>();
            if (!GameModeManager.IsMultiplayer || (view != null && view.IsMine))
            {
                return player;
            }
        }

        return null;
    }
}
