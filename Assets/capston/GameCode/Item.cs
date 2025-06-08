using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type { Coin, Heart, BulletDamageUp, FireRateUp, BulletCountUp }
    public Type type;

    public float value = 100f;

    void Update()
    {
        if (!GameModeManager.IsMultiplayer)
        {
            UpgradeStatsManager.Instance.LoadUpgradeLevels();  // �ֽ� ���׷��̵� ���� �ε�
        }

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
                    float multiplier = 1f;

                    // �̱� ����� ���� ���� ���׷��̵� ȿ�� ����
                    if (!GameModeManager.IsMultiplayer)
                    {
                        multiplier += UpgradeStatsManager.Instance?.GetBonusCoinAmount() ?? 1f;
                    }

                    int total = Mathf.RoundToInt(value * multiplier);
                    Debug.Log($"������ ȹ���߽��ϴ�. +{total} (�⺻: {value}, ����: {multiplier:F1})");
                    CoinManager.Instance.AddCoin(total);
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
