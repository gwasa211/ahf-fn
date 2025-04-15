using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingPlatform : MonoBehaviour
{
    public float slideSpeed = 5f; // �̲����� �ӵ�

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // ĳ���Ϳ� �浹 ��
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                // �̲������� �̵�
                playerRb.velocity = new Vector2(slideSpeed, playerRb.velocity.y);
            }
        }
    }
}
