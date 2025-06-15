using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    public Transform hostSpawnPoint;
    public Transform clientSpawnPoint;

    void Start()
    {
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        string prefabName;
        Vector3 spawnPos;

        if (PhotonNetwork.IsMasterClient)
        {
            prefabName = "Host"; // 대문자 H
            spawnPos = hostSpawnPoint.position;
        }
        else
        {
            prefabName = "client"; // 소문자 c
            spawnPos = clientSpawnPoint.position;
        }

        GameObject player = PhotonNetwork.Instantiate(prefabName, spawnPos, Quaternion.identity);

        //  내 플레이어라면 BossSpawner 생성 (멀티 전용)
        if (GameModeManager.IsMultiplayer && player.GetComponent<PhotonView>().IsMine)
        {
            Instantiate(Resources.Load("BossSpawner"));
            Debug.Log("[PlayerSpawner] 내 BossSpawner 생성 완료");
        }
    }
}
