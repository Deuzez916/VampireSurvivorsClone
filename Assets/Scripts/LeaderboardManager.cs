using TMPro;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    public TMP_Text bestTimeText;
    public TMP_Text bestKillsText;

    void Start()
    {
        float bestTime = PlayerPrefs.GetFloat("BestTime", 0f);
        int bestKills = PlayerPrefs.GetInt("BestKills", 0);

        bestTimeText.text = FormatTime(bestTime);
        bestKillsText.text = bestKills.ToString();
    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}