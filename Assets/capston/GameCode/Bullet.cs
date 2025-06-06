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
        // 충돌한 오브젝트 로그 찍기
        Debug.Log($"[Bullet] 충돌한 오브젝트: {other.name}");

        // 자식 오브젝트까지 포함해서 Enemy 탐색
        Enemy enemy = other.GetComponentInParent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);

            Destroy(gameObject); // 총알 제거
        }
    }
}
