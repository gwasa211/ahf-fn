using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class dpsjal : MonoBehaviour
{
    public float moveSpeed = 2f;         // �¿� �̵� �ӵ�
    public float jumpForce = 5f;         // ���� ��
    public float jumpInterval = 2f;      // ���� ���� (��)


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
        // �¿� �̵�
        float moveDir = movingRight ? 1f : -1f;
        rb.velocity = new Vector2(moveDir * moveSpeed, rb.velocity.y);

        // ���� Ÿ�̸�
        jumpTimer -= Time.deltaTime;
        if (jumpTimer <= 0f)
        {
            Jump();
            jumpTimer = jumpInterval;
        }
    }

    void Jump()
    {
        // ���� ������ ���� (�����ϰ� y �ӵ��� ���� 0�� ���� üũ)
        if (Mathf.Abs(rb.velocity.y) < 0.01f)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    // ���̳� ���� ������ ���� ��ȯ(������ ��)


private void OnCollisionEnter2D(Collision2D collision)
    {
        // �÷��̾�ʹ� �������� �ʰ�, "Wall" �Ǵ� "Ground" �±׿��� ����
        if (collision.collider.CompareTag("Wall"))
        {
            movingRight = !movingRight;

            // �¿� ���� (�� ��������Ʈ ������)
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
}