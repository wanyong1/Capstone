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

        PhotonNetwork.Instantiate(prefabName, spawnPos, Quaternion.identity);
    }
}
