using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacteOccupation2 : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;  // 점프력 약간 증가

    public Transform[] groundChecks;
    public LayerMask groundLayer;

    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;

    [Header("Arrow Rain Settings")]
    public Vector3[] arrowSpawnOffsets;
    public GameObject arrowPrefab;
    public float arrowFallSpeed = 10f;

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

    private float lastSummonTime = -Mathf.Infinity;
    private float summonCooldown = 16f;

    private bool isInvincible = false;
    private float invincibleDuration = 3f;

    [Header("R 스킬 - 보호막 (8초 유지)")]
    public GameObject objectToActivate;
    public float activateDuration = 8f;

    [Header("E 스킬 - 산탄총 발사체")]
    public GameObject shotgunProjectilePrefab;
    public float shotgunProjectileSpeed = 8f;
    public float shotgunSpreadAngle = 45f;
    public float shotgunDestroyDelay = 6f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;

        // Rigidbody2D 설정 점검용 로그
        if (rb.bodyType != RigidbodyType2D.Dynamic)
            Debug.LogWarning("Rigidbody2D Body Type이 Dynamic인지 확인하세요.");
        if (rb.gravityScale <= 0)
            Debug.LogWarning("Rigidbody2D Gravity Scale이 0보다 큰지 확인하세요.");

        currentLives = maxLives;
    }

    private void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        // 땅 체크
        isGrounded = false;
        foreach (Transform groundCheck in groundChecks)
        {
            if (Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer))
            {
                isGrounded = true;
                break;
            }
        }

        bool jumpKeyPressed = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.C);
        bool canJump = isGrounded && jumpKeyPressed && Time.time >= lastJumpTime + jumpCooldown;

        Debug.Log($"isGrounded: {isGrounded}, jumpKeyPressed: {jumpKeyPressed}, canJump: {canJump}");

        if (canJump)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            lastJumpTime = Time.time;
        }

        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        HandleShotgunFire();   // E 스킬
        HandleSummonSkill();   // R 스킬
        UpdateFacingDirection(moveInput);
    }

    // ---------- E 스킬 (산탄총) ----------
    private void HandleShotgunFire()
    {
        if (Input.GetKeyDown(KeyCode.E) && Time.time >= lastProjectileTime + projectileCooldown)
        {
            FireShotgun();
            lastProjectileTime = Time.time;
        }
    }

    private void FireShotgun()
    {
        if (shotgunProjectilePrefab == null) return;

        Vector3 spawnPos = transform.position + Vector3.up;
        float[] angles = { -shotgunSpreadAngle, -shotgunSpreadAngle / 2, 0f, shotgunSpreadAngle / 2, shotgunSpreadAngle, 90f };

        foreach (float angle in angles)
        {
            float baseAngle = angle + (transform.localScale.x < 0 ? 180f : 0f);
            Quaternion rot = Quaternion.Euler(0f, 0f, baseAngle);
            GameObject proj = Instantiate(shotgunProjectilePrefab, spawnPos, rot);

            Rigidbody2D rbProj = proj.GetComponent<Rigidbody2D>();
            if (rbProj != null)
            {
                Vector2 direction = rot * Vector2.right;
                rbProj.bodyType = RigidbodyType2D.Dynamic;
                rbProj.isKinematic = false;
                rbProj.velocity = direction * shotgunProjectileSpeed;
            }

            Destroy(proj, shotgunDestroyDelay);
        }
    }

    // ---------- R 스킬 (보호막) ----------
    private void HandleSummonSkill()
    {
        if (Input.GetKeyDown(KeyCode.R) && Time.time >= lastSummonTime + summonCooldown)
        {
            ActivateObjectTemporarily();
            lastSummonTime = Time.time;
        }
    }

    private void ActivateObjectTemporarily()
    {
        if (objectToActivate == null) return;

        objectToActivate.SetActive(true);
        isInvincible = true;

        StartCoroutine(DeactivateAfterDelay(objectToActivate, activateDuration));
    }

    private IEnumerator DeactivateAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
        isInvincible = false;
    }

    private void LoseLife()
    {
        if (isInvincible) return;

        currentLives--;

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
            if (bossAttackHitCount >= maxBossHits)
            {
                SceneManager.LoadScene("Level_4.5");
            }
        }
        else if (collision.CompareTag("Finish"))
        {
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
        if (moveInput > 0 && transform.localScale.x < 0)
        {
            Flip();
        }
        else if (moveInput < 0 && transform.localScale.x > 0)
        {
            Flip();
        }
    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
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
