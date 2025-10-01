using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelBar : MonoBehaviour
{
    public PlayerController player;
    public Image fillImage;
    public TMP_Text xpText;

    void Update()
    {
        float fillAmount = (float)player.currentXP / player.xpToNextLevel;
        fillImage.fillAmount = fillAmount;

        if (xpText != null)
        {
            xpText.text = player.currentXP + " / " + player.xpToNextLevel + " XP";
        }
    }
}
