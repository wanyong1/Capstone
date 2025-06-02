using UnityEngine;
using Photon.Pun;
using System.Collections;

public class Enemy : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    public float moveSpeed = 3f;
    public int maxHP = 10;
    private int currentHP;

    public GameObject expOrbPrefab;
    public GameObject coinPrefab;

    private Transform target;

    public bool isFromOtherPlayer = false;
    private bool isDying = false;

    void Start()
    {
        currentHP = maxHP;

        if (isFromOtherPlayer)
            ApplyRedColor();

        StartCoroutine(AssignTargetClosest());
    }

    IEnumerator AssignTargetClosest()
    {
        while (target == null)
        {
            float minDist = Mathf.Infinity;
            Player closest = null;

            Player[] allPlayers = FindObjectsByType<Player>(FindObjectsSortMode.None);
            foreach (var player in allPlayers)
            {
                float dist = Vector3.Distance(transform.position, player.transform.position);
                if (dist < minDist)
                {
                    closest = player;
                    minDist = dist;
                }
            }

            if (closest != null)
            {
                target = closest.transform;
                break;
            }

            yield return null;
        }
    }

    void Update()
    {
        if (target == null) return;

        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;

        if (dir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(dir);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
                playerHealth.TakeDamage(3);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDying) return;

        currentHP -= damage;
        Debug.Log($"[Enemy] {gameObject.name} 남은 체력: {currentHP}");
        if (currentHP <= 0)
        {
            isDying = true;
            Die();
        }
    }

    void Die()
    {
        try
        {
            Debug.Log($"[Enemy] Die() 호출됨 - 이름: {gameObject.name}, IsMine: {(photonView != null ? photonView.IsMine.ToString() : "null")}, fromOther: {isFromOtherPlayer}");

            // 상대방에게 몬스터 정보 전달
            if (GameModeManager.IsMultiplayer && photonView != null && photonView.IsMine && !isFromOtherPlayer)
            {
                string prefabName = gameObject.name.Replace("(Clone)", "");
                Debug.Log($"[Enemy] 내가 소환한 몬스터 제거됨 → '{prefabName}' 을 RPC로 상대방에게 전달 시도");

                PhotonView managerView = FindFirstObjectByType<MonsterSpawner>()?.GetComponent<PhotonView>();
                if (managerView != null)
                    managerView.RPC("SpawnEnemyFromOtherPlayer", RpcTarget.Others, prefabName);
                else
                    Debug.LogWarning("[Enemy] MonsterSpawner의 PhotonView를 찾지 못했습니다!");
            }

            // 경험치 드랍
            if (!isFromOtherPlayer && expOrbPrefab != null)
            {
                if (!GameModeManager.IsMultiplayer)
                {
                    Instantiate(expOrbPrefab, transform.position, Quaternion.identity);
                }
                else if (photonView != null && photonView.IsMine)
                {
                    PhotonNetwork.Instantiate("Exp", transform.position, Quaternion.identity);
                }
            }

            // 코인 드랍 (싱글 전용)
            if (!GameModeManager.IsMultiplayer && coinPrefab != null && !isFromOtherPlayer)
            {
                Instantiate(coinPrefab, transform.position + Vector3.right * 0.5f, Quaternion.identity);
            }

            // 제거 처리
            if (GameModeManager.IsMultiplayer)
            {
                if (photonView != null && !photonView.IsMine)
                {
                    photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
                }
                StartCoroutine(DestroyAfterOwnershipTransfer());
            }
            else
            {
                Destroy(gameObject);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[ERROR] Die() 중 예외 발생: {e.Message}");
        }
    }

    IEnumerator DestroyAfterOwnershipTransfer()
    {
        yield return new WaitForSeconds(0.05f);
        if (photonView != null && photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    [PunRPC]
    public void SetAsFromOtherPlayer()
    {
        isFromOtherPlayer = true;
        ApplyRedColor();
        StartCoroutine(AssignTargetClosest());
    }

    public void ApplyRedColor()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in renderers)
        {
            foreach (Material mat in rend.materials)
            {
                mat.color = Color.black;
            }
        }
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info) { }
}
