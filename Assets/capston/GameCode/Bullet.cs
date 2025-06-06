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
        // �浹�� ������Ʈ �α� ���
        Debug.Log($"[Bullet] �浹�� ������Ʈ: {other.name}");

        // �ڽ� ������Ʈ���� �����ؼ� Enemy Ž��
        Enemy enemy = other.GetComponentInParent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);

            Destroy(gameObject); // �Ѿ� ����
        }
    }
}
