using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float baseMoveSpeed = 5f;
    public float boostedMoveSpeed = 8f;
    public float jumpForce = 5f;
    public Transform[] groundChecks;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float jumpCooldown = 0.6f;
    private float lastJumpTime = 0f;

    private bool isInvincible = false;      // 무적 상태 변수
    private bool isSpeedBoosted = false;    // 이동 속도 증가 상태 변수

    private float moveSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        moveSpeed = baseMoveSpeed;
    }

    private void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

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
    }

    // 무적 스위치 ON
    public void TurnOnInvincible()
    {
        isInvincible = true;
        Debug.Log("무적 상태 ON");
        // 무적 관련 추가 처리 (예: 피해 무시 등)
    }

    // 이동 속도 증가 스위치 ON
    public void TurnOnSpeedBoost()
    {
        isSpeedBoosted = true;
        moveSpeed = boostedMoveSpeed;
        Debug.Log("이동 속도 증가 ON");
    }

    // 충돌 처리 예시 (수정 필요 시 참고)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
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

        // 기타 충돌 처리...
    }
}





