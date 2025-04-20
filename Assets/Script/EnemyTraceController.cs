using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTraceController : MonoBehaviour
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

    private Rigidbody2D rb;
    private float jumpTimer;
    private bool movingRight = true;
    private Transform player;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpTimer = jumpInterval;
        rb.freezeRotation = true;

        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
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
        if (collision.CompareTag("PlayerProjectile"))  // 또는 "Attack"
        {
            TakeDamage(50); // 예시로 50 데미지
            Destroy(collision.gameObject); // 투사체 제거
        }
    }

    private void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"적 체력: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("적이 사망했습니다.");
        Destroy(gameObject);
    }
}