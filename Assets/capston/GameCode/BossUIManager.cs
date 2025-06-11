using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossUIManager : MonoBehaviour
{
    public static BossUIManager Instance;

    public GameObject panel;          // BossPanel
    public Slider bossHpSlider;      // BossHPBar
    public TMP_Text bossHpText;      // BossHPText

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            panel.SetActive(false); // 시작 시 비활성화
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Show(float maxHP)
    {
        if (panel != null) panel.SetActive(true);
        if (bossHpSlider != null)
        {
            bossHpSlider.maxValue = maxHP;
            bossHpSlider.value = maxHP;
        }

        if (bossHpText != null)
            bossHpText.text = $"{maxHP} / {maxHP}";
    }

    public void UpdateHP(float current, float max)
    {
        if (bossHpSlider != null)
            bossHpSlider.value = current;

        if (bossHpText != null)
            bossHpText.text = $"{current} / {max}";
    }

    public void Hide()
    {
        if (panel != null) panel.SetActive(false);
    }
}
