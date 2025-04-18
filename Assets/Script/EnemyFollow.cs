using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Transform player;       // �÷��̾� Transform (Inspector���� ����)
    public float moveSpeed = 3f;   // �̵� �ӵ�

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;  // ȸ�� ���� (�ʿ��)
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        // �÷��̾� ���� ���� ���
        Vector2 direction = (player.position - transform.position).normalized;

        // Rigidbody2D�� �̿��� �̵�
        rb.velocity = direction * moveSpeed;
    }
}