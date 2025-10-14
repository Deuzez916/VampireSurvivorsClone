using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 8f;
    public int damage = 1;
    public float lifetime = 5f;
    public float currentLifeTimer = 0f;
    public string shooterTag;

    private Vector2 moveDirection;

    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        currentLifeTimer = 0f;
    }

    void Update()
    {
        if (StageManager.Instance.IsPaused || StageManager.Instance.IsUpgrading)
            return;

        transform.position += (Vector3)(moveDirection * speed * Time.deltaTime);
        
        if (currentLifeTimer >= lifetime)
        {
            Destroy(gameObject);
        } 
        else
        {
            currentLifeTimer += Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(shooterTag)) return;

        if (shooterTag == "Player" && other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.health -= damage;
                Soundmanager.Instance.PlaySoundEffect(SoundEffects.EnemyHit);
            }
            Destroy(gameObject);
        }
        else if (shooterTag == "Enemy" && other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
