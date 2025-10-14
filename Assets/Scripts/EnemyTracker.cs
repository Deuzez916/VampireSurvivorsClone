using UnityEngine;

public class EnemyTracker : MonoBehaviour
{
    public static EnemyTracker Instance;

    public int killCount = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddKill() => killCount++;
    public int GetKillCount() => killCount;
}
