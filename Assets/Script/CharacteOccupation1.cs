using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class YourCharacterScript : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    public Transform[] groundChecks;
    public LayerMask groundLayer;

    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;

    [Header("Arrow Rain Settings")]
    public Vector3[] arrowSpawnOffsets;
    public GameObject arrowPrefab;
    public float arrowFallSpeed = 10f;

    [Header("UI Elements")]
    public Image skillCooldownImage;
    public Text skillCooldownText;
    public Text lifeText;

    public int maxLives = 3;
    private int currentLives;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float jumpCooldown = 0.6f;
    private float lastJumpTime = 0f;

    private int bossAttackHitCount = 0;
    private int maxBossHits = 3;

    private float lastProjectileTime = -Mathf.Infinity;
    private float projectileCooldown = 1f;

    private float lastArrowRainTime = -Mathf.Infinity;
    private float arrowRainCooldown = 10f;

    private bool isInvincible = false;
    private float invincibleDuration = 3f;

    private SpriteRenderer spriteRenderer;
    private bool facingRight = true;
    float score;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        currentLives = maxLives;
        UpdateLifeUI();
    }

    private void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        isGrounded = false;
        foreach (Transform groundCheck in groundChecks)
        {
            if (Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer))
            {
                isGrounded = true;
                break;
            }
        }

        if (isGrounded && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.C)) && Time.time >= lastJumpTime + jumpCooldown)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            lastJumpTime = Time.time;
        }

        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        UpdateFacingDirection(moveInput);
        UpdateSkillCooldownUI();
        HandleProjectileFire();
        HandleArrowRain();
    }

    private void HandleProjectileFire()
    {
        if (Input.GetKeyDown(KeyCode.E) && Time.time >= lastProjectileTime + projectileCooldown)
        {
            FireProjectile();
            lastProjectileTime = Time.time;
        }
    }

    private void FireProjectile()
    {
        if (projectilePrefab == null) return;

        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + 1f, 0f);
        GameObject projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

        projectile.transform.localScale *= 4f;

        Rigidbody2D projRb = projectile.GetComponent<Rigidbody2D>();
        if (projRb != null)
        {
            projRb.bodyType = RigidbodyType2D.Dynamic;
            projRb.isKinematic = false;
            float direction = spriteRenderer.flipX ? -1f : 1f;
            projRb.velocity = Vector2.right * projectileSpeed * direction;
        }

        Destroy(projectile, 5f);
    }

    private void HandleArrowRain()
    {
        if (Input.GetKeyDown(KeyCode.R) && Time.time >= lastArrowRainTime + arrowRainCooldown)
        {
            StartCoroutine(ArrowRainRoutine());
            lastArrowRainTime = Time.time;
        }
    }

    private IEnumerator ArrowRainRoutine()
    {
        if (arrowSpawnOffsets == null || arrowSpawnOffsets.Length == 0 || arrowPrefab == null)
            yield break;

        int totalDrops = 10;

        for (int i = 0; i < totalDrops; i++)
        {
            List<Vector3> shuffledOffsets = new List<Vector3>(arrowSpawnOffsets);
            ShuffleList(shuffledOffsets);

            foreach (Vector3 offset in shuffledOffsets)
            {
                Vector3 spawnPos = transform.position + offset;
                SpawnArrow(spawnPos);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    private void SpawnArrow(Vector3 spawnPos)
    {
        GameObject arrow = Instantiate(arrowPrefab, spawnPos, Quaternion.Euler(0f, 0f, -90f));

        arrow.transform.localScale *= 5f;

        Rigidbody2D arrowRb = arrow.GetComponent<Rigidbody2D>();
        if (arrowRb != null)
        {
            arrowRb.bodyType = RigidbodyType2D.Dynamic;
            arrowRb.isKinematic = false;
            arrowRb.velocity = Vector2.down * arrowFallSpeed;
        }

        Destroy(arrow, 5f);
    }

    private void UpdateSkillCooldownUI()
    {
        if (skillCooldownImage != null)
        {
            float cooldownRatio = Mathf.Clamp01((Time.time - lastArrowRainTime) / arrowRainCooldown);
            skillCooldownImage.fillAmount = cooldownRatio;
        }

        if (skillCooldownText != null)
        {
            float remaining = Mathf.Max(0, arrowRainCooldown - (Time.time - lastArrowRainTime));
            skillCooldownText.text = remaining > 0 ? remaining.ToString("F1") + "초" : "사용 가능";
        }
    }

    private void UpdateLifeUI()
    {
        if (lifeText != null)
            lifeText.text = $"목숨: {currentLives}";
    }

    private void LoseLife()
    {
        if (isInvincible) return;

        currentLives--;
        UpdateLifeUI();

        if (currentLives <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            StartCoroutine(InvincibleCoroutine());
        }
    }

    private IEnumerator InvincibleCoroutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleDuration);
        isInvincible = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isInvincible) return;

        if (collision.CompareTag("Enemy") || collision.CompareTag("Respawn"))
        {
            LoseLife();
        }
        else if (collision.CompareTag("BossAttack"))
        {
            bossAttackHitCount++;
            Debug.Log($"보스 공격에 맞음: {bossAttackHitCount}회");

            if (bossAttackHitCount >= maxBossHits)
            {
                Debug.Log("보스 공격 3회 충돌 - 씬 이동");
                SceneManager.LoadScene("Level_4.5");
            }
        }
        else if (collision.CompareTag("Finish"))
        {
            HighScore.Tryset(SceneManager.GetActiveScene().buildIndex, (int)score);
            var levelObject = collision.GetComponent<LevelObject>();
            if (levelObject != null)
            {
                levelObject.MoveToNextLevel();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isInvincible) return;

        if (collision.collider.CompareTag("Enemy"))
        {
            LoseLife();
        }
    }

    private void UpdateFacingDirection(float moveInput)
    {
        if (moveInput > 0)
        {
            spriteRenderer.flipX = false;
            facingRight = true;
        }
        else if (moveInput < 0)
        {
            spriteRenderer.flipX = true;
            facingRight = false;
        }
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
