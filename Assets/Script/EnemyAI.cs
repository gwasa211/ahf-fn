using UnityEngine;
using UnityEngine.UI; // ← UI를 위해 추가

public class EnemyAI : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float jumpForce = 5f;
    public float jumpInterval = 2f;

    [Header("Detection Settings")]
    public float traceDistance = 5f;
    public float raycastDistance = 2f;

    [Header("Health Settings")]
    public int maxHealth = 200;
    private int currentHealth;

    [Header("On Death Action")]
    public GameObject objectToActivate;

    [Header("UI Settings")]
    public Slider healthSlider; // ✅ 보스 체력 슬라이더

    private Rigidbody2D rb;
    private float jumpTimer;
    private bool movingRight = true;
    private Transform player;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        jumpTimer = jumpInterval;

        currentHealth = maxHealth;

        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = maxHealth;
        }
    }

    private void Update()
    {
        if (player == null)
        {
            PatrolMove();
            return;
        }

        Vector2 directionToPlayer = player.position - transform.position;

        if (directionToPlayer.magnitude <= traceDistance)
        {
            FollowPlayer(directionToPlayer);
        }
        else
        {
            PatrolMove();
        }

        HandleJump();
    }

    private void FollowPlayer(Vector2 direction)
    {
        Vector2 directionNormalized = direction.normalized;

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, directionNormalized, raycastDistance);
        Debug.DrawRay(transform.position, directionNormalized * raycastDistance, Color.red);

        bool obstacleDetected = false;
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.CompareTag("Wall"))
            {
                obstacleDetected = true;
                break;
            }
        }

        if (obstacleDetected)
        {
            Vector3 alternativeDirection = Quaternion.Euler(0, 0, -90) * direction;
            rb.velocity = new Vector2(alternativeDirection.x * moveSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(directionNormalized.x * moveSpeed, rb.velocity.y);
        }

        if (directionNormalized.x > 0 && !movingRight)
        {
            Flip();
        }
        else if (directionNormalized.x < 0 && movingRight)
        {
            Flip();
        }
    }

    private void PatrolMove()
    {
        float moveDir = movingRight ? 1f : -1f;
        rb.velocity = new Vector2(moveDir * moveSpeed, rb.velocity.y);
    }

    private void HandleJump()
    {
        jumpTimer -= Time.deltaTime;
        if (jumpTimer <= 0f && Mathf.Abs(rb.velocity.y) < 0.01f)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpTimer = jumpInterval;
        }
    }

    private void Flip()
    {
        movingRight = !movingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            Flip();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerProjectile1"))
        {
            TakeDamage(5);
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("PlayerProjectile2"))
        {
            TakeDamage(1);
            Destroy(collision.gameObject);
        }
    }

    private void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log($"적 체력: {currentHealth}");

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("적이 사망했습니다.");

        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
        }

        Destroy(gameObject);
    }
}
