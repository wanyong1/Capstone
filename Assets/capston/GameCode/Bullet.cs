using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // 총알 속도
    public float lifeTime = 3f; // 총알이 사라지는 시간
    public float damage = 5f;

    void Start()
    {
        Destroy(gameObject, lifeTime); // 일정 시간 후 총알 삭제

    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime); // 전방으로 이동
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[Bullet] 충돌한 오브젝트: {other.name}");

        // 1. 보스 처리 (tag로 판단)
        if (other.CompareTag("boss"))
        {
            BossHealth boss = other.GetComponentInParent<BossHealth>();
            if (boss != null)
            {
                boss.TakeDamage(damage);
            }
            else
            {
                Debug.LogWarning("[Bullet] Boss 태그는 있지만 BossHealth가 없습니다.");
            }

            Destroy(gameObject);
            return;
        }

        // 2. 일반 몬스터 처리
        Enemy enemy = other.GetComponentInParent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }
    }

}
