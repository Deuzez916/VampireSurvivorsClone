using UnityEngine;

public class XPOrb : MonoBehaviour
{
    public int xpAmount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.GainXP(xpAmount);
                Destroy(gameObject);
            }
        }
    }
}
