using UnityEngine;

public class UpgradeStatsManager : MonoBehaviour
{
    public static UpgradeStatsManager Instance;

    public int BulletDamageLevel { get; private set; }
    public int MoveSpeedLevel { get; private set; }
    public int ExpBonusLevel { get; private set; }
    public int CoinBonusLevel { get; private set; }
    public int MaxHealthLevel { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadUpgradeLevels();
    }

    public  void LoadUpgradeLevels()
    {
        BulletDamageLevel = PlayerPrefs.GetInt("BulletDamageUpgradeLevel", 0);
        MoveSpeedLevel = PlayerPrefs.GetInt("MoveSpeedUpgradeLevel", 0);
        ExpBonusLevel = PlayerPrefs.GetInt("ExpBonusUpgradeLevel", 0);
        CoinBonusLevel = PlayerPrefs.GetInt("CoinBonusUpgradeLevel", 0);
        MaxHealthLevel = PlayerPrefs.GetInt("MaxHealthUpgradeLevel", 0);


        //BulletDamageLevel = 0;
        //MoveSpeedLevel = 0;
        //ExpBonusLevel = 0;
        //CoinBonusLevel = 0;
        //MaxHealthLevel = 0;

        Debug.Log($"[UpgradeStatsManager] 업그레이드 레벨 로드 완료 - BulletDamage: {BulletDamageLevel}, MoveSpeed: {MoveSpeedLevel}, Exp: {ExpBonusLevel}, Coin: {CoinBonusLevel}, Health: {MaxHealthLevel}");
    }

    // 실제 수치를 반환하는 메서드 예시
    public float GetBulletDamageBonus()
    {
        return BulletDamageLevel * 1f; 
    }

    public float GetMoveSpeedBonus()
    {
        return MoveSpeedLevel * 0.3f; // 레벨당 0.3 유닛 증가
    }

    public float GetBonusCoinAmount()
    {
        return 1f + (CoinBonusLevel * 0.1f); // 레벨당 10% 증가
    }

    public int GetBonusExpAmount()
    {
        return ExpBonusLevel; // 레벨당 +1 경험치
    }

    public int GetExtraHealth()
    {
        return MaxHealthLevel * 5; // 레벨당 * 5, ex) 레벨 3이면 3 * 5 = 15
    }
}
