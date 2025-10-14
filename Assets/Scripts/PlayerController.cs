using System;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.XR;

public class PlayerController : MonoBehaviour
{
    public bool canMove = true;

    public float moveSpeed = 5f;
    public int health = 10;
    public int damage = 1;

    public int level = 1;
    public int currentXP = 0;
    public int xpToNextLevel = 10;

    public GameObject projectilePrefab;
    private Vector2 lastMoveDirection = Vector2.right;
    private Vector2 currentMoveInput;

    [SerializeField] private ParticleSystem hitEffect;

    public int weaponLevel = 0;
    public int maxWeaponLevel = 4;

    public int selfHealLevel = 0;
    public int maxSelfHealLevel = 2;
    private bool hasSelfHeal = false;
    private float healTimer = 0f;
    private float healDealy = 3f;
    private float healInterval = 3f;
    private int healAmount = 1;


    public int sprintLevel = 0;
    public int maxSprintLevel = 10;

    private UpgradeManager upgradeManager;

    void Start()
    {
        upgradeManager = FindObjectOfType<UpgradeManager>();
    }

    void Update()
    {
        if (StageManager.Instance.IsPaused || StageManager.Instance.IsUpgrading)
        {
            if (TryGetComponent<Animator>(out Animator anim))
                anim.speed = 0;
            return;
        }
        else
        {
            if (TryGetComponent<Animator>(out Animator anim))
                anim.speed = 1;
        }

        Move();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
            Soundmanager.Instance.PlaySoundEffect(SoundEffects.PlayerShoot);
        }

        if (hasSelfHeal)
        {
            HandleSelfHeal();
        }

        CheckDeath();
    }

    void Move()
    {
        if (!canMove) return;

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        currentMoveInput = new Vector2(moveX, moveY).normalized;

        if (currentMoveInput != Vector2.zero)
        {
            lastMoveDirection = currentMoveInput;
            healTimer = 0f;
        }

        float speedMultiplier = 1f;
        if (sprintLevel >= 1)
        {
            speedMultiplier += 0.1f * sprintLevel;
        }

        speedMultiplier = Mathf.Min(speedMultiplier, 2f);

        float speed = moveSpeed * speedMultiplier;
        transform.position += (Vector3)(currentMoveInput * speed * Time.deltaTime);
    }

    void Shoot()
    {
        if (!canMove) return;

        Vector2 backDirection = -lastMoveDirection;

        Vector2 side1 = Vector2.zero;
        Vector2 side2 = Vector2.zero;


        if (Mathf.Abs(lastMoveDirection.x) > Mathf.Abs(lastMoveDirection.y))
        {
            side1 = Vector2.up;
            side2 = Vector2.down;
        }
        else if (Mathf.Abs(lastMoveDirection.y) > Mathf.Abs(lastMoveDirection.x))
        {
            side1 = Vector2.right;
            side2 = Vector2.left;
        }

        SpawnProjectile(lastMoveDirection, 0f);

        if (weaponLevel >= 1) SpawnProjectile(backDirection, 0.2f);
        if (weaponLevel >= 2) SpawnProjectile(lastMoveDirection, -0.2f);
        if (weaponLevel >= 3) SpawnProjectile(backDirection, -0.2f);
        if (weaponLevel >= 4)
        {
            SpawnProjectile(side1, 0f);
            SpawnProjectile(side2, 0f);
        }
    }

    void SpawnProjectile(Vector2 direction, float offset)
    {
        Vector3 spawnPos = transform.position;

        if (direction == Vector2.right || direction == Vector2.left || direction == -Vector2.right || direction == -Vector2.left)
            spawnPos += new Vector3(0f, offset, 0f);

        else if (direction == Vector2.up || direction == Vector2.down || direction == -Vector2.up || direction == -Vector2.down)
            spawnPos += new Vector3(offset, 0f, 0f);

        GameObject projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        projectile.GetComponent<Projectile>().SetDirection(direction);
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

        if (upgradeManager != null)
        {
            upgradeManager.ShowUpgradeMenu();
        }

        Soundmanager.Instance.PlaySoundEffect(SoundEffects.LevelUp);
    }

    public void UpgradeWeapon()
    {
        if (weaponLevel < maxWeaponLevel)
        {
            weaponLevel++;
        }
    }

    public void UpgradeSprint()
    {
        if (sprintLevel < maxSprintLevel)
        {
            sprintLevel++;
        }
    }

    public void UpgradeSelfHeal()
    {
        if (selfHealLevel < maxSelfHealLevel)
        {
            selfHealLevel++;
            hasSelfHeal = true;

            if (selfHealLevel == 1)
            {
                healDealy = 3f;
                healInterval = 3f;
                healAmount = 1;
            }
            else if (selfHealLevel == 2)
            {
                healDealy = 3f;
                healInterval = 2f;
                healAmount = 2;
            }
        }
    }

    void HandleSelfHeal()
    {
        if (currentMoveInput == Vector2.zero)
        {
            healTimer += Time.deltaTime;

            if (healTimer >= healDealy + healInterval)
            {
                health += healAmount;
                healTimer = healDealy;
            }
        }
        else
        {
            healTimer = 0f;
        }
    }

    public void TakeDamage(int amount)
    {
        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }

        health -= amount;
        Debug.Log("Player Health: " + health);
    }

    void CheckDeath()
    {
        if (health <= 0)
        {
            canMove = false;
            StageManager.Instance.SetState(GameState.GameOver);

            Soundmanager.Instance.PlaySoundEffect(SoundEffects.PlayerDeath);

            Soundmanager.Instance.PlayBackground(SoundEffects.GameOver);

            SavePlayerStats();
        }
    }

    void SavePlayerStats()
    {
        float timePlayed = Time.timeSinceLevelLoad;
        int kills = EnemyTracker.Instance != null ? EnemyTracker.Instance.GetKillCount() : 0;

        float bestTime = PlayerPrefs.GetFloat("BestTime", 0f);
        int bestKills = PlayerPrefs.GetInt("BestKills", 0);

        if (timePlayed > bestTime)
            PlayerPrefs.SetFloat("BestTime", timePlayed);

        if (kills > bestKills)
            PlayerPrefs.SetInt("BestKills", kills);
            
        PlayerPrefs.Save();
    }
}
