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
            prefabName = "Host"; // �빮�� H
            spawnPos = hostSpawnPoint.position;
        }
        else
        {
            prefabName = "client"; // �ҹ��� c
            spawnPos = clientSpawnPoint.position;
        }

        GameObject player = PhotonNetwork.Instantiate(prefabName, spawnPos, Quaternion.identity);

        //  �� �÷��̾��� BossSpawner ���� (��Ƽ ����)
        if (GameModeManager.IsMultiplayer && player.GetComponent<PhotonView>().IsMine)
        {
            Instantiate(Resources.Load("BossSpawner"));
            Debug.Log("[PlayerSpawner] �� BossSpawner ���� �Ϸ�");
        }
    }
}
