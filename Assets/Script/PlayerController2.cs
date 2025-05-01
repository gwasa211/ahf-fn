using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerController2 : MonoBehaviour
{
    public float moveSpeed = 5f; // 이동 속도
    public float jumpForce = 5f; // 점프 힘
    public Transform[] groundChecks; // 여러 개의 GroundCheck
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float jumpCooldown = 0.6f; // 쿨타임 설정
    private float lastJumpTime = 0f; // 마지막 점프 시간

    // 스위치 변수 추가
  
    private bool isInvincible = false;
    float score;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; // 회전 고정
        score = 1000f;
    }

    private void Update()
    {
        MoveRight(); // 자동으로 오른쪽으로 이동
        CheckGround(); // 바닥 체크
        Jump(); // 점프 처리
    }

    private void MoveRight()
    {
        // 항상 오른쪽으로 이동
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
    }

    private void CheckGround()
    {
        isGrounded = false; // 바닥 체크 초기화
        foreach (Transform groundCheck in groundChecks)
        {
            // 각 groundCheck에서 바닥을 체크
            if (Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer))
            {
                isGrounded = true;
                break; // 하나라도 바닥에 닿으면 true로 설정
            }
        }
    }

    private void Jump()
    {
        // 현재 시간과 마지막 점프 시간 비교
        if (isGrounded && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.C)) && Time.time >= lastJumpTime + jumpCooldown)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            lastJumpTime = Time.time; // 점프 시간 업데이트
        }
    }

    // 무적 상태 활성화 함수
    public void TurnOnInvincible()
    {
        isInvincible = true;
        Debug.Log("무적 상태 ON");
        // 필요시 무적 효과 지속 시간 처리 추가 가능
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Respawn"))
        {
            if (isInvincible)
            {
                Destroy(collision.gameObject);
                Debug.Log("무적 상태 - 적 파괴");
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        if (collision.CompareTag("Finish"))
        {
            HighScore.Tryset(SceneManager.GetActiveScene().buildIndex, (int)score);
            collision.GetComponent<LevelObject>().MoveToNextLevel();
        }
    }
}
