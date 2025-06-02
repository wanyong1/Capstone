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
                    Debug.Log("코인을 획득했습니다. +" + value);
                    CoinManager.Instance.AddCoin(value); //  여기에 코인 증가 코드 추가
                }
                else
                {
                    player.ApplyItem(type, value);
                }

                Destroy(gameObject);
            }
        }
    }
}
