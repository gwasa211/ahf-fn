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

    private bool isInvincible = false;      // ���� ���� ����
    private bool isSpeedBoosted = false;    // �̵� �ӵ� ���� ���� ����

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

    // ���� ����ġ ON
    public void TurnOnInvincible()
    {
        isInvincible = true;
        Debug.Log("���� ���� ON");
        // ���� ���� �߰� ó�� (��: ���� ���� ��)
    }

    // �̵� �ӵ� ���� ����ġ ON
    public void TurnOnSpeedBoost()
    {
        isSpeedBoosted = true;
        moveSpeed = boostedMoveSpeed;
        Debug.Log("�̵� �ӵ� ���� ON");
    }

    // �浹 ó�� ���� (���� �ʿ� �� ����)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
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

        // ��Ÿ �浹 ó��...
    }
}





