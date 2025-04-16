using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 플레이어 스크립트 가져오기
            PlayerController2 player = collision.GetComponent<PlayerController2>();
            if (player != null)
            {
                player.TurnOnSwitch(); // 스위치 켜기
            }

            Destroy(gameObject); // 아이템 파괴
        }
    }
}
