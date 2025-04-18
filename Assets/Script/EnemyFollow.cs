using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Transform player;       // 플레이어 Transform (Inspector에서 연결)
    public float moveSpeed = 3f;   // 이동 속도

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;  // 회전 고정 (필요시)
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        // 플레이어 방향 벡터 계산
        Vector2 direction = (player.position - transform.position).normalized;

        // Rigidbody2D를 이용해 이동
        rb.velocity = direction * moveSpeed;
    }
}