using UnityEngine;

public class OrbitingObject : MonoBehaviour
{
    public Transform target; // 회전할 대상 (플레이어)
    public float orbitDistance = 2f;
    public float orbitSpeed = 180f; // 도/초
    public int orbitDamage = 5;

    private float angle;

    void Update()
    {
        if (target == null) return;

        angle += orbitSpeed * Time.deltaTime;
        if (angle > 360f) angle -= 360f;

        float rad = angle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad)) * orbitDistance;
        transform.position = target.position + offset;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(orbitDamage);
            }
        }
    }
}
