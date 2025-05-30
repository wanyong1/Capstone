using UnityEngine;
using TMPro;

public class MonsterCounter : MonoBehaviour
{
    public Vector3 hostFieldCenter = new Vector3(0, 0, 0);
    public Vector3 clientFieldCenter = new Vector3(850, 0, 850);
    public Vector3 fieldSize = new Vector3(500, 300, 500); // 필드 scale 기준
    public LayerMask monsterLayer;

    public TMP_Text hostCountText;
    public TMP_Text clientCountText;

    private float logCooldown = 5f;
    private float logTimer = 0f;

    private void Update()
    {
        int hostCount = CountMonstersInField(hostFieldCenter);
        int clientCount = CountMonstersInField(clientFieldCenter);

        hostCountText.text = $"MY Monsters: {hostCount}";
        clientCountText.text = $"Opponent Monsters: {clientCount}";

        logTimer += Time.deltaTime;
        if (logTimer >= logCooldown)
        {
            Debug.Log($"[MonsterFieldCount] Host Field: {hostCount} | Client Field: {clientCount}");
            logTimer = 0f;
        }
    }

    int CountMonstersInField(Vector3 center)
    {
        Collider[] hits = Physics.OverlapBox(center, fieldSize / 2f, Quaternion.identity, monsterLayer);
        return hits.Length;
    }

    // Gizmo로 시각적 디버깅
    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Matrix4x4 oldMatrix = Gizmos.matrix;

        // Host 필드 (Y축으로 45도 회전)
        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(hostFieldCenter, Quaternion.Euler(0, 45, 0), Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, fieldSize);

        // Client 필드 (Y축으로 45도 회전)
        Gizmos.color = Color.blue;
        Gizmos.matrix = Matrix4x4.TRS(clientFieldCenter, Quaternion.Euler(0, 45, 0), Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, fieldSize);

        Gizmos.matrix = oldMatrix;
#endif
    }
}
