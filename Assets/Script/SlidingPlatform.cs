using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingPlatform : MonoBehaviour
{
    public float slideSpeed = 5f; // 미끄러짐 속도

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // 캐릭터와 충돌 시
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                // 미끄러지게 이동
                playerRb.velocity = new Vector2(slideSpeed, playerRb.velocity.y);
            }
        }
    }
}
