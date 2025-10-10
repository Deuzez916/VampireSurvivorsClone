using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy2Controller : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float attackRange = 6f;
    public float coolDownTimer = 4f;
    public GameObject bulletPrefab;
    public float bulletSpeed = 7f;

    public Transform Player;
    private bool canAttack = true;
    private bool canMove = true;

    public Enemy enemyStats;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;

        if (enemyStats == null)
        {
            enemyStats = GetComponent<Enemy>();
        }
    }

    void Update()
    {
        if (StageManager.Instance.IsPaused || StageManager.Instance.IsUpgrading) return;
        if (Player == null) return;

        if (enemyStats.health <= 0)
        {
            XPDroppable xpDrop = GetComponent<XPDroppable>();
            if (xpDrop != null)
            {
                xpDrop.DropXP();
            }
            Destroy(gameObject);
        }

        float distance = Vector2.Distance(transform.position, Player.position);

        if (distance <= attackRange && canAttack)
        {
            StartCoroutine(ShootAtPlayer());
        }

        if (canMove)
        {
            MoveTowardsPlayer();
        }
    }


    void MoveTowardsPlayer()
    {
        Vector2 direction = (Player.position - transform.position).normalized;
        transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);
    }

    IEnumerator ShootAtPlayer()
    {
        canAttack = false;
        canMove = false;

        Vector3 targetPos = Player.position;
        Vector2 direction = (targetPos - transform.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        
        Projectile proj = bullet.GetComponent<Projectile>();
        if (proj != null)
        {
            proj.shooterTag = "Enemy";
            proj.SetDirection(direction);
        }

        yield return new WaitForSeconds(coolDownTimer);

        canMove = true;
        canAttack = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null && enemyStats != null)
            {
                player.TakeDamage(enemyStats.damage);
            }
        }
    }   
}
