using UnityEngine;
using Photon.Pun;

public class BossSpawner : MonoBehaviour
{
    [Header("�̱� ���� ���� ������")]
    public GameObject bossPrefabSingle;

    [Header("���� ��ȯ ��� �ð� (��)")]
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
        // �̹� ������ �ִٸ� ��ȯ���� ����
        if (GameObject.FindWithTag("boss") != null)
        {
            Debug.Log("[BossSpawner] ������ �̹� �����Ͽ� ��ȯ ����");
            return;
        }

        GameObject myPlayer = FindMyPlayer();
        if (myPlayer == null)
        {
            Debug.LogWarning("[BossSpawner] �� �÷��̾ ã�� ����");
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

        Debug.Log($"[BossSpawner] ���� ��ȯ �Ϸ� @ {spawnPos}");
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
