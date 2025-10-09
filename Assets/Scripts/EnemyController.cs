using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private PlayerController playerController;

    public float moveSpeed = 2f;
    public float attackCooldown = 1f;
    public float lastAttackTime;

    [Header("XP Drop")]
    public GameObject[] xpPrefab;

    private Transform playerTransform;
    private Enemy enemyStats;
    private Rigidbody2D rb;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerController = playerObj.GetComponent<PlayerController>();
            playerTransform = playerObj.transform;
        }

        enemyStats = GetComponent<Enemy>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (StageManager.Instance.IsPaused || StageManager.Instance.IsUpgrading) return;
        if (playerController == null) return;
        if (!playerController.canMove) return;

        MoveTowardsPlayer();
        ChechDeath();
    }

    void MoveTowardsPlayer()
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);
    }

    void ChechDeath()
    {
        if (enemyStats.health <= 0)
        {
            DropXP();
            Destroy(gameObject);
            Debug.Log("Enemy defeated");
        }
    }

    void DropXP()
    {
        if (xpPrefab.Length > 0)
        {
            int randomIndex = Random.Range(0, xpPrefab.Length);
            Instantiate(xpPrefab[randomIndex], transform.position, Quaternion.identity);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Time.time > lastAttackTime + attackCooldown)
        {
            PlayerController playerStatus = other.GetComponent<PlayerController>();
            if (playerStatus != null)
            {
                playerStatus.TakeDamage(enemyStats.damage);
                lastAttackTime = Time.time;
            }
        }
    }
}
