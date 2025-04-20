using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacteOccupation3 : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;  // ������ �ణ ����

    public Transform[] groundChecks;
    public LayerMask groundLayer;

    [Header("E Skill Settings")]
    public GameObject trackingProjectilePrefab;  // ���� ������Ʈ ������
    public int trackingProjectilesCount = 4;
    public float trackingProjectileSpawnRadiusX = 3f;
    public float trackingProjectileSpawnRadiusY = 2f;
    public float eSkillCooldown = 2f;

    [Header("R Skill Settings")]
    public GameObject pushProjectilePrefab;      // �о�� ������Ʈ ������
    public float pushProjectileSpeed = 15f;
    public float rSkillCooldown = 7f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float jumpCooldown = 0.6f;
    private float lastJumpTime = 0f;

    private float lastESkillTime = -Mathf.Infinity;
    private float lastRSkillTime = -Mathf.Infinity;

    private int currentLives;
    public int maxLives = 3;

    private bool isInvincible = false;
    private float invincibleDuration = 3f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;

        if (rb.gravityScale == 0)
            Debug.LogWarning("Rigidbody2D Gravity Scale�� 0���� �����Ǿ� �ֽ��ϴ�. ������ �� �� �� �ֽ��ϴ�.");

        if (rb.bodyType != RigidbodyType2D.Dynamic)
            Debug.LogWarning("Rigidbody2D Body Type�� Dynamic���� Ȯ���ϼ���.");
    }

    private void Start()
    {
        currentLives = maxLives;
        UpdateLifeUI();
    }

    private void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        // �� üũ
        isGrounded = false;
        foreach (Transform groundCheck in groundChecks)
        {
            if (Physics2D.OverlapCircle(groundCheck.position, 0.3f, groundLayer))
            {
                isGrounded = true;
                break;
            }
        }

        // ���� ���� ����� �α�
        bool jumpKeyPressed = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.C);
        bool canJumpCooldown = Time.time >= lastJumpTime + jumpCooldown;
        Debug.Log($"isGrounded: {isGrounded}, JumpKeyPressed: {jumpKeyPressed}, CanJumpCooldown: {canJumpCooldown}");

        // ����
        if (isGrounded && jumpKeyPressed && canJumpCooldown)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            lastJumpTime = Time.time;
        }

        // �̵�
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // E ��ų �ߵ�
        if (Input.GetKeyDown(KeyCode.E) && Time.time >= lastESkillTime + eSkillCooldown)
        {
            CastESkill();
            lastESkillTime = Time.time;
        }

        // R ��ų �ߵ�
        if (Input.GetKeyDown(KeyCode.R) && Time.time >= lastRSkillTime + rSkillCooldown)
        {
            CastRSkill();
            lastRSkillTime = Time.time;
        }

        UpdateFacingDirection(moveInput);
    }

    private void CastESkill()
    {
        if (trackingProjectilePrefab == null) return;

        for (int i = 0; i < trackingProjectilesCount; i++)
        {
            float offsetX = Random.Range(-trackingProjectileSpawnRadiusX, trackingProjectileSpawnRadiusX);
            float offsetY = Random.Range(-trackingProjectileSpawnRadiusY, trackingProjectileSpawnRadiusY);
            Vector3 spawnPos = transform.position + new Vector3(offsetX, offsetY, 0f);

            GameObject proj = Instantiate(trackingProjectilePrefab, spawnPos, Quaternion.identity);

            TrackingProjectile trackingProj = proj.GetComponent<TrackingProjectile>();
            if (trackingProj != null)
            {
                GameObject targetEnemy = FindNearestEnemy(proj.transform.position);
                if (targetEnemy != null)
                {
                    trackingProj.SetTarget(targetEnemy.transform);
                }
                else
                {
                    Destroy(proj, 5f);
                }
            }
        }
    }

    private GameObject FindNearestEnemy(Vector3 fromPosition)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearest = null;
        float minDist = Mathf.Infinity;

        foreach (var enemy in enemies)
        {
            float dist = Vector3.Distance(fromPosition, enemy.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = enemy;
            }
        }

        return nearest;
    }

    private void CastRSkill()
    {
        if (pushProjectilePrefab == null) return;

        float directionX = transform.localScale.x > 0 ? 1f : -1f;
        Vector3 spawnPos = transform.position + new Vector3(directionX * 2f, 2f, 0f); // ���� 2ĭ, ���� 2ĭ

        GameObject proj = Instantiate(pushProjectilePrefab, spawnPos, Quaternion.identity);

        // ũ�� ���� (1.5��)
        proj.transform.localScale *= 4f;

        Rigidbody2D projRb = proj.GetComponent<Rigidbody2D>();
        if (projRb != null)
        {
            projRb.velocity = Vector2.right * directionX * pushProjectileSpeed;
        }

        Destroy(proj, 5f);
    }

    private void UpdateLifeUI()
    {
        // UI ���� �ʿ� �� ����
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
}
