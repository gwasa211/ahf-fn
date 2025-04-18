using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class dpsjal : MonoBehaviour
{
    public float moveSpeed = 2f;         // 좌우 이동 속도
    public float jumpForce = 5f;         // 점프 힘
    public float jumpInterval = 2f;      // 점프 간격 (초)


    private Rigidbody2D rb;
    private float jumpTimer;

    private bool movingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpTimer = jumpInterval;
        rb.freezeRotation = true;
    }

    void Update()
    {
        // 좌우 이동
        float moveDir = movingRight ? 1f : -1f;
        rb.velocity = new Vector2(moveDir * moveSpeed, rb.velocity.y);

        // 점프 타이머
        jumpTimer -= Time.deltaTime;
        if (jumpTimer <= 0f)
        {
            Jump();
            jumpTimer = jumpInterval;
        }
    }

    void Jump()
    {
        // 땅에 있으면 점프 (간단하게 y 속도가 거의 0인 경우로 체크)
        if (Mathf.Abs(rb.velocity.y) < 0.01f)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    // 벽이나 끝에 닿으면 방향 전환(간단한 예)


private void OnCollisionEnter2D(Collision2D collision)
    {
        // 플레이어와는 반응하지 않고, "Wall" 또는 "Ground" 태그에만 반응
        if (collision.collider.CompareTag("Wall"))
        {
            movingRight = !movingRight;

            // 좌우 반전 (적 스프라이트 뒤집기)
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
}