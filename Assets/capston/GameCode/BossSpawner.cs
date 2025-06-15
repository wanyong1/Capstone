using UnityEngine;
using Photon.Pun;

public class BossSpawner : MonoBehaviour
{
    [Header("싱글 모드용 프리팹")]
    public GameObject bossPrefabSingle;

    [Header("보스 타이머 설정")]
    public float spawnDelay = 300f;

    private float timer = 0f;
    private bool bossAlive = false;

    void Update()
    {
        // 멀티 모드일 경우 내 스포너가 아니면 작동 X
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
            Debug.Log("[BossSpawner] 이미 보스가 존재합니다.");
            return;
        }

        GameObject myPlayer = FindMyPlayer();
        if (myPlayer == null)
        {
            Debug.LogWarning("[BossSpawner] 내 플레이어를 찾을 수 없습니다.");
            return;
        }

        Vector3 spawnPos = myPlayer.transform.position + new Vector3(30f, 0f, 0f);

        if (GameModeManager.IsMultiplayer)
        {
            Instantiate(Resources.Load("Boss"), spawnPos, Quaternion.identity); // 로컬 인스턴스
        }
        else
        {
            Instantiate(bossPrefabSingle, spawnPos, Quaternion.identity);
        }

        bossAlive = true;
        Debug.Log($"[BossSpawner] 보스가 소환되었습니다 @ {spawnPos}");
    }

    public void NotifyBossDeath()
    {
        if (!IsMySpawner()) return;

        bossAlive = false;
        Debug.Log("[BossSpawner] 보스 사망 확인 → 타이머 재시작");
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
