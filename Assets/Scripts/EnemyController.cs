using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private PlayerController playerController;

    public float moveSpeed = 2f;
    public float attackCooldown = 1f;
    public float lastAttackTime;

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

        if (enemyStats.health <= 0)
        {
            XPDroppable xpDrop = GetComponent<XPDroppable>();
            if (xpDrop != null)
            {
                xpDrop.DropXP();
            }
            Destroy(gameObject);
        }

        MoveTowardsPlayer();
        CheckDeath();
    }

    void MoveTowardsPlayer()
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);
    }

    void CheckDeath()
    {
        if (enemyStats.health <= 0)
        {
            XPDroppable xpDrop = GetComponent<XPDroppable>();
            if (xpDrop != null)
            {
                xpDrop.DropXP();
            }
            Destroy(gameObject);
            Debug.Log("Enemy defeated");
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
