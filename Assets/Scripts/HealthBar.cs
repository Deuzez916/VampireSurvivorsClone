using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public PlayerController player;
    public Image healthFill;

    private int maxHealth;

    void Start()
    {
        maxHealth = player.health;
    }

    void Update()
    {
        if (player != null && healthFill != null)
        {
            healthFill.fillAmount = (float)player.health / maxHealth;
        }
    }
}
