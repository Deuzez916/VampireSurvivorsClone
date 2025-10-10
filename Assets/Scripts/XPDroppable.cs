using UnityEngine;

public class XPDroppable : MonoBehaviour
{
    [Header("XP Drop")]
    public GameObject[] xpPrefab;

    public void DropXP()
    {
        if (xpPrefab.Length == 0) return;
        {
            int randomIndex = Random.Range(0, xpPrefab.Length);
            Instantiate(xpPrefab[randomIndex], transform.position, Quaternion.identity);
        }
    }
}
