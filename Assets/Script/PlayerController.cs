using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float baseMoveSpeed = 5f;
    public float boostedMoveSpeed = 8f;

    public float baseJumpForce = 5f;
    public float boostedJumpForce = 8f;

    public Transform[] groundChecks;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float jumpCooldown = 0.6f;
    private float lastJumpTime = 0f;

    private bool isInvincible = false;
    private bool isSpeedBoosted = false;
    private bool isJumpBoosted = false;

    private float moveSpeed;

    private Coroutine jumpBoostCoroutine;

    public Animator myAnimator;

    private bool isMoving = false;  // 현재 이동 상태 저장용

    float score;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        moveSpeed = baseMoveSpeed;

        if (myAnimator == null)
            myAnimator = GetComponent<Animator>();

        if (myAnimator != null)
            myAnimator.SetBool("Move", false);
        score = 1000f;
    }

    private void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        // 이동 속도 결정
        moveSpeed = isSpeedBoosted ? boostedMoveSpeed : baseMoveSpeed;

        // 땅 체크
        isGrounded = false;
        foreach (Transform groundCheck in groundChecks)
        {
            Collider2D collider = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
            if (collider != null)
            {
                isGrounded = true;
                break;
            }
        }

        // 방향 전환
        if (moveInput > 0)
        {
            transform.localScale = new Vector3(3, 3, 1);
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-3, 3, 1);
        }

        // 이동 상태 변경시만 애니메이터 파라미터 변경 (불필요한 호출 방지)
        bool currentlyMoving = moveInput != 0;
        if (currentlyMoving != isMoving)
        {
            isMoving = currentlyMoving;
            if (myAnimator != null)
                myAnimator.SetBool("Move", isMoving);
        }

        // 이동 처리
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // 점프력 결정
        float currentJumpForce = isJumpBoosted ? boostedJumpForce : baseJumpForce;

        // 점프 입력 및 쿨타임 처리
        if (isGrounded && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.C)) && Time.time >= lastJumpTime + jumpCooldown)
        {
            rb.AddForce(Vector2.up * currentJumpForce, ForceMode2D.Impulse);
            lastJumpTime = Time.time;
        }
        score -= Time.deltaTime;
    }

    // 무적 상태 ON
    public void TurnOnInvincible()
    {
        isInvincible = true;
        Debug.Log("무적 상태 ON");
    }

    // 무적 상태 OFF (추가)
    public void TurnOffInvincible()
    {
        isInvincible = false;
        Debug.Log("무적 상태 OFF");
    }

    // 이동 속도 증가 ON
    public void TurnOnSpeedBoost()
    {
        isSpeedBoosted = true;
        Debug.Log("이동 속도 증가 ON");
    }

    // 점프력 증가 ON (지속시간 지정)
    public void TurnOnJumpBoost(float duration)
    {
        if (jumpBoostCoroutine != null)
            StopCoroutine(jumpBoostCoroutine);

        jumpBoostCoroutine = StartCoroutine(JumpBoostRoutine(duration));
    }

    private IEnumerator JumpBoostRoutine(float duration)
    {
        isJumpBoosted = true;
        Debug.Log($"점프력 증가 ON ({duration}초)");

        yield return new WaitForSeconds(duration);

        isJumpBoosted = false;
        Debug.Log("점프력 증가 OFF");
    }

    // 충돌 처리
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
        if (collision.collider.CompareTag("Enemy"))
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
    }
}
