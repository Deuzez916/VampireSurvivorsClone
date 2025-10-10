using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [System.Serializable]
    public class EnemySpawnInfo
    {
        public GameObject prefab;
        public int maxCount = 10;
    }
    public EnemySpawnInfo[] enemies;
    public float spawnInterval = 2f;
    public int minEnemies = 1;
    public int maxEnemies = 5;
    public float spawnOffset = 5f;

    private Camera mainCam;
    private float timer;

    void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        if (StageManager.Instance.IsPaused || StageManager.Instance.IsUpgrading) return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnEnemies();
            timer = 0f;
        }
    }

    void SpawnEnemies()
    {
        int enemyCount = Random.Range(minEnemies, maxEnemies + 1);

        for (int i = 0; i < enemyCount; i++)
        {
            int enemyIndex = Random.Range(0, enemies.Length);
            EnemySpawnInfo info = enemies[enemyIndex];

            if (info.maxCount > 0)
            {
                int currentCount = CountEnemiesOfType(info.prefab.name);
                if (currentCount >= info.maxCount)
                    continue;
            }

            Vector3 spawnPos = GetRandomSpawnPosition();
            GameObject enemy = Instantiate(info.prefab, spawnPos, Quaternion.identity);

            enemy.tag = "Enemy";
        }
    }

    int CountEnemiesOfType(string prefabName)
    {
        int count = 0;
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (enemy.name.Contains(prefabName))
                count++;
        }
        return count;
    }

    Vector3 GetRandomSpawnPosition()
    {
        float height = 2f * mainCam.orthographicSize;
        float width = height * mainCam.aspect;

        float halfWidth = width / 2f;
        float halfHeight = height / 2f;

        int side = Random.Range(0, 4);
        Vector3 spawnPos = Vector3.zero;

        switch (side)
        {
            case 0: // Left
                spawnPos = new Vector3(mainCam.transform.position.x - halfWidth - spawnOffset,
                    Random.Range(mainCam.transform.position.y - halfHeight, mainCam.transform.position.y + halfHeight),
                    0);
                break;
            case 1: // Right
                spawnPos = new Vector3(mainCam.transform.position.x + halfWidth + spawnOffset,
                    Random.Range(mainCam.transform.position.y - halfHeight, mainCam.transform.position.y + halfHeight),
                    0);
                break;
            case 2: // Top
                spawnPos = new Vector3(Random.Range(mainCam.transform.position.x - halfWidth, mainCam.transform.position.x + halfWidth),
                    mainCam.transform.position.y + halfHeight + spawnOffset,
                    0);
                break;
            case 3: // Bottom
                spawnPos = new Vector3(Random.Range(mainCam.transform.position.x - halfWidth, mainCam.transform.position.x + halfWidth),
                    mainCam.transform.position.y - halfHeight - spawnOffset,
                    0);
                break;
        }
        return spawnPos;
    }
}
