using UnityEngine;

public class ExpOrb : MonoBehaviour
{
    public int expAmount = 1;

    void Update()
    {
        transform.Rotate(Vector3.up * 60 * Time.deltaTime); // 회전 효과
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            Debug.Log("ExpOrb 충돌 발생!");

            PlayerExp exp = other.GetComponent<PlayerExp>();
            if (exp != null)
            {
                exp.AddExp(expAmount); // 플레이어에 경험치 전달
                Vector3 dropPosition = transform.position + new Vector3(Random.Range(-1f, 1f), 1.5f, Random.Range(-1f, 1f));
            }

            Destroy(gameObject);
        }
    }
}
