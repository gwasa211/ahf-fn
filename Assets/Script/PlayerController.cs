using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{


public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public Transform[] groundChecks; // 여러 개의 GroundCheck
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float jumpCooldown = 0.6f; // 쿨타임 설정
    private float lastJumpTime = 0f; // 마지막 점프 시간

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; // 회전 고정
    }

    private void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

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

        // 현재 시간과 마지막 점프 시간 비교
        if (isGrounded && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.C)) && Time.time >= lastJumpTime + jumpCooldown)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            lastJumpTime = Time.time; // 점프 시간 업데이트
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Respawn"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        {
            if (collision.CompareTag("Respawn"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

            if (collision.CompareTag("Finish"))
            {
                collision.GetComponent<LevelObject>().MoveToNextLevel();
            }
        }
    }
    

}
