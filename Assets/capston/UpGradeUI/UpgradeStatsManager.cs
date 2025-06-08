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

        Debug.Log($"[UpgradeStatsManager] ���׷��̵� ���� �ε� �Ϸ� - BulletDamage: {BulletDamageLevel}, MoveSpeed: {MoveSpeedLevel}, Exp: {ExpBonusLevel}, Coin: {CoinBonusLevel}, Health: {MaxHealthLevel}");
    }

    // ���� ��ġ�� ��ȯ�ϴ� �޼��� ����
    public float GetBulletDamageBonus()
    {
        return BulletDamageLevel * 1f; 
    }

    public float GetMoveSpeedBonus()
    {
        return MoveSpeedLevel * 0.3f; // ������ 0.3 ���� ����
    }

    public float GetBonusCoinAmount()
    {
        return 1f + (CoinBonusLevel * 0.1f); // ������ 10% ����
    }

    public int GetBonusExpAmount()
    {
        return ExpBonusLevel; // ������ +1 ����ġ
    }

    public int GetExtraHealth()
    {
        return MaxHealthLevel * 5; // ������ * 5, ex) ���� 3�̸� 3 * 5 = 15
    }
}
