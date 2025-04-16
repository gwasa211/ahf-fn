using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private bool isSwitchOn = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; // 회전 고정
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

    // 아이템 먹었을 때 호출할 함수 추가
    public void TurnOnSwitch()
    {
        isSwitchOn = true;
        Debug.Log("스위치 ON");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Respawn"))
        {
            if (isSwitchOn)
            {
                // 스위치가 켜져 있으면 오브젝트 파괴
                Destroy(collision.gameObject);
                Debug.Log("스위치 ON 상태, Respawn 오브젝트 파괴");
            }
            else
            {
                // 스위치가 꺼져 있으면 씬 재로딩
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        if (collision.CompareTag("Finish"))
        {
            collision.GetComponent<LevelObject>().MoveToNextLevel();
        }
    }
}
