using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController2 : MonoBehaviour
{
    public float moveSpeed = 5f; // �̵� �ӵ�
    public float jumpForce = 5f; // ���� ��
    public Transform[] groundChecks; // ���� ���� GroundCheck
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float jumpCooldown = 0.6f; // ��Ÿ�� ����
    private float lastJumpTime = 0f; // ������ ���� �ð�

    // ����ġ ���� �߰�
    private bool isSwitchOn = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; // ȸ�� ����
    }

    private void Update()
    {
        MoveRight(); // �ڵ����� ���������� �̵�
        CheckGround(); // �ٴ� üũ
        Jump(); // ���� ó��
    }

    private void MoveRight()
    {
        // �׻� ���������� �̵�
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
    }

    private void CheckGround()
    {
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
    }

    private void Jump()
    {
        // ���� �ð��� ������ ���� �ð� ��
        if (isGrounded && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.C)) && Time.time >= lastJumpTime + jumpCooldown)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            lastJumpTime = Time.time; // ���� �ð� ������Ʈ
        }
    }

    // ������ �Ծ��� �� ȣ���� �Լ� �߰�
    public void TurnOnSwitch()
    {
        isSwitchOn = true;
        Debug.Log("����ġ ON");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Respawn"))
        {
            if (isSwitchOn)
            {
                // ����ġ�� ���� ������ ������Ʈ �ı�
                Destroy(collision.gameObject);
                Debug.Log("����ġ ON ����, Respawn ������Ʈ �ı�");
            }
            else
            {
                // ����ġ�� ���� ������ �� ��ε�
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        if (collision.CompareTag("Finish"))
        {
            collision.GetComponent<LevelObject>().MoveToNextLevel();
        }
    }
}
