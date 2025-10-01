using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 8f;
    public int damage = 1;
    public float lifetime = 5f;

    private Vector2 moveDirection;

    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction.normalized;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.health -= damage;
            }
        }
    }
}
