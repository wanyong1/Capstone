using UnityEngine;
using Photon.Pun;

public class BossSpawner : MonoBehaviour
{
    [Header("�̱� ���� ������")]
    public GameObject bossPrefabSingle;

    [Header("���� Ÿ�̸� ����")]
    public float spawnDelay = 300f;

    private float timer = 0f;
    private bool bossAlive = false;

    void Update()
    {
        // ��Ƽ ����� ��� �� �����ʰ� �ƴϸ� �۵� X
        if (!IsMySpawner()) return;

        if (bossAlive) return;

        timer += Time.deltaTime;

        if (timer >= spawnDelay)
        {
            SpawnBoss();
            timer = 0f;
        }
    }

    void SpawnBoss()
    {
        if (bossAlive || GameObject.FindWithTag("boss") != null)
        {
            Debug.Log("[BossSpawner] �̹� ������ �����մϴ�.");
            return;
        }

        GameObject myPlayer = FindMyPlayer();
        if (myPlayer == null)
        {
            Debug.LogWarning("[BossSpawner] �� �÷��̾ ã�� �� �����ϴ�.");
            return;
        }

        Vector3 spawnPos = myPlayer.transform.position + new Vector3(30f, 0f, 0f);

        if (GameModeManager.IsMultiplayer)
        {
            Instantiate(Resources.Load("Boss"), spawnPos, Quaternion.identity); // ���� �ν��Ͻ�
        }
        else
        {
            Instantiate(bossPrefabSingle, spawnPos, Quaternion.identity);
        }

        bossAlive = true;
        Debug.Log($"[BossSpawner] ������ ��ȯ�Ǿ����ϴ� @ {spawnPos}");
    }

    public void NotifyBossDeath()
    {
        if (!IsMySpawner()) return;

        bossAlive = false;
        Debug.Log("[BossSpawner] ���� ��� Ȯ�� �� Ÿ�̸� �����");
    }

    GameObject FindMyPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var p in players)
        {
            PhotonView view = p.GetComponent<PhotonView>();
            if (!GameModeManager.IsMultiplayer || (view != null && view.IsMine))
                return p;
        }
        return null;
    }

    bool IsMySpawner()
    {
        if (!GameModeManager.IsMultiplayer) return true;
        PhotonView view = GetComponent<PhotonView>();
        return view == null || view.IsMine;
    }
}
