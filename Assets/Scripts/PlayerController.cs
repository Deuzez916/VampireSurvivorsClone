using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public int health = 10;
    public int damage = 1;

    public int level = 1;

    public int currentXP = 0;

    public int xpToNextLevel = 10;
    public GameObject projectilePrefab;

    private Vector2 lastMoveDirection = Vector2.right;

    void Update()
    {
        Move();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }

        CheckDeath();
    }

    void Move()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector2 moveDirection = new Vector2(moveX, moveY).normalized;

        if (moveDirection != Vector2.zero)
        {
            lastMoveDirection = moveDirection;
        }

        transform.position += (Vector3)moveDirection * moveSpeed * Time.deltaTime;
    }

    void Shoot()
    {
        GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        proj.GetComponent<Projectile>().SetDirection(lastMoveDirection);
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        Debug.Log("Player Health: " + health);
    }

    void CheckDeath()
    {
        if (health <= 0)
        {
            Debug.Log("Player Deid!");
            Destroy(gameObject);
        }
    }

    public void GainXP(int amount)
    {
        currentXP += amount;

        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        level++;
        currentXP -= xpToNextLevel;
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.5f);
    }
}
