using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float attackCooldown = 1f;
    public float lastAttackTime;


    private Transform player;
    private Enemy enemyStats;
    private Rigidbody2D rb;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyStats = GetComponent<Enemy>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (player == null) return;
        MoveTowardsPlayer();
        ChechDeath();
    }

    void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);
    }

    void ChechDeath()
    {
        if (enemyStats.health <= 0)
        {
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
