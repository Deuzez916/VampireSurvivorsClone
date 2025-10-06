using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefab;
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
        int enemyIndex;

        for (int i = 0; i < enemyCount; i++)
        {
            Vector3 spawnPos = GetRandomSpawnPosition();
            enemyIndex = Random.Range(0, enemyPrefab.Length);
            Instantiate(enemyPrefab[enemyIndex], spawnPos, Quaternion.identity);
        }
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
