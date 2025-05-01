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

    private bool isMoving = false;  // ���� �̵� ���� �����

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

        // �̵� �ӵ� ����
        moveSpeed = isSpeedBoosted ? boostedMoveSpeed : baseMoveSpeed;

        // �� üũ
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

        // ���� ��ȯ
        if (moveInput > 0)
        {
            transform.localScale = new Vector3(3, 3, 1);
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-3, 3, 1);
        }

        // �̵� ���� ����ø� �ִϸ����� �Ķ���� ���� (���ʿ��� ȣ�� ����)
        bool currentlyMoving = moveInput != 0;
        if (currentlyMoving != isMoving)
        {
            isMoving = currentlyMoving;
            if (myAnimator != null)
                myAnimator.SetBool("Move", isMoving);
        }

        // �̵� ó��
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // ������ ����
        float currentJumpForce = isJumpBoosted ? boostedJumpForce : baseJumpForce;

        // ���� �Է� �� ��Ÿ�� ó��
        if (isGrounded && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.C)) && Time.time >= lastJumpTime + jumpCooldown)
        {
            rb.AddForce(Vector2.up * currentJumpForce, ForceMode2D.Impulse);
            lastJumpTime = Time.time;
        }
        score -= Time.deltaTime;
    }

    // ���� ���� ON
    public void TurnOnInvincible()
    {
        isInvincible = true;
        Debug.Log("���� ���� ON");
    }

    // ���� ���� OFF (�߰�)
    public void TurnOffInvincible()
    {
        isInvincible = false;
        Debug.Log("���� ���� OFF");
    }

    // �̵� �ӵ� ���� ON
    public void TurnOnSpeedBoost()
    {
        isSpeedBoosted = true;
        Debug.Log("�̵� �ӵ� ���� ON");
    }

    // ������ ���� ON (���ӽð� ����)
    public void TurnOnJumpBoost(float duration)
    {
        if (jumpBoostCoroutine != null)
            StopCoroutine(jumpBoostCoroutine);

        jumpBoostCoroutine = StartCoroutine(JumpBoostRoutine(duration));
    }

    private IEnumerator JumpBoostRoutine(float duration)
    {
        isJumpBoosted = true;
        Debug.Log($"������ ���� ON ({duration}��)");

        yield return new WaitForSeconds(duration);

        isJumpBoosted = false;
        Debug.Log("������ ���� OFF");
    }

    // �浹 ó��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Respawn"))
        {
            if (isInvincible)
            {
                Destroy(collision.gameObject);
                Debug.Log("���� ���� - �� �ı�");
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
                Debug.Log("���� ���� - �� �ı�");
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
