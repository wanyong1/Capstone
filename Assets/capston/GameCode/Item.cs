using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type { Coin, Heart, BulletDamageUp, FireRateUp, BulletCountUp }
    public Type type;

    public int value = 1;

    void Update()
    {
        transform.Rotate(Vector3.up * 30 * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                if (type == Type.Coin)
                {
                    Debug.Log(" 코인을 획득했습니다. +" + value);
                    // 필요하면 코인 저장 시스템 추가 가능
                }
                else
                {
                    player.ApplyItem(type, value); // 능력치 적용
                }

                Destroy(gameObject); // 아이템 제거
            }
        }
    }
}
