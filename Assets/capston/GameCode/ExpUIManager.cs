using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class ExpUIManager : MonoBehaviour
{
    public Image expFillImage;
    public TMP_Text expText;
    public TMP_Text levelText;

    private PlayerExp playerExp;

    void Start()
    {
        StartCoroutine(FindMyPlayerExp());
    }

    System.Collections.IEnumerator FindMyPlayerExp()
    {
        while (playerExp == null)
        {
            var exps = FindObjectsByType<PlayerExp>(FindObjectsSortMode.None);
            foreach (var exp in exps)
            {
                var view = exp.GetComponent<PhotonView>();
                if (view == null || view.IsMine)
                {
                    playerExp = exp;
                    break;
                }
            }
            yield return null;
        }
    }

    void Update()
    {
        if (playerExp == null) return;

        if (expFillImage != null)
        {
            float fillAmount = (float)playerExp.currentExp / playerExp.expToNextLevel;
            expFillImage.fillAmount = fillAmount;
        }

        if (expText != null)
        {
            expText.text = $"EXP: {playerExp.currentExp} / {playerExp.expToNextLevel}";
        }

        if (levelText != null)
        {
            levelText.text = $"Lv. {playerExp.level}";
        }
    }
}
