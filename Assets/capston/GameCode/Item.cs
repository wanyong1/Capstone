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
            UpgradeStatsManager.Instance.LoadUpgradeLevels();  // 최신 업그레이드 정보 로드
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

                    // 싱글 모드일 때만 상점 업그레이드 효과 적용
                    if (!GameModeManager.IsMultiplayer)
                    {
                        multiplier += UpgradeStatsManager.Instance?.GetBonusCoinAmount() ?? 1f;
                    }

                    int total = Mathf.RoundToInt(value * multiplier);
                    Debug.Log($"코인을 획득했습니다. +{total} (기본: {value}, 배율: {multiplier:F1})");
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
