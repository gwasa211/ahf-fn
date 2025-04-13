using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{


public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public Transform[] groundChecks; // ���� ���� GroundCheck
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float jumpCooldown = 0.6f; // ��Ÿ�� ����
    private float lastJumpTime = 0f; // ������ ���� �ð�

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; // ȸ�� ����
    }

    private void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        isGrounded = false; // �ٴ� üũ �ʱ�ȭ
        foreach (Transform groundCheck in groundChecks)
        {
            // �� groundCheck���� �ٴ��� üũ
            if (Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer))
            {
                isGrounded = true;
                break; // �ϳ��� �ٴڿ� ������ true�� ����
            }
        }

        // ���� �ð��� ������ ���� �ð� ��
        if (isGrounded && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.C)) && Time.time >= lastJumpTime + jumpCooldown)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            lastJumpTime = Time.time; // ���� �ð� ������Ʈ
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
