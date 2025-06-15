using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // �Ѿ� �ӵ�
    public float lifeTime = 3f; // �Ѿ��� ������� �ð�
    public float damage = 5f;

    void Start()
    {
        Destroy(gameObject, lifeTime); // ���� �ð� �� �Ѿ� ����

    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime); // �������� �̵�
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[Bullet] �浹�� ������Ʈ: {other.name}");

        // 1. ���� ó�� (tag�� �Ǵ�)
        if (other.CompareTag("boss"))
        {
            BossHealth boss = other.GetComponentInParent<BossHealth>();
            if (boss != null)
            {
                boss.TakeDamage(damage);
            }
            else
            {
                Debug.LogWarning("[Bullet] Boss �±״� ������ BossHealth�� �����ϴ�.");
            }

            Destroy(gameObject);
            return;
        }

        // 2. �Ϲ� ���� ó��
        Enemy enemy = other.GetComponentInParent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }
    }

}
