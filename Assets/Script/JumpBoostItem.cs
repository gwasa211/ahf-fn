using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBoostItem : MonoBehaviour
{
    public GameObject effectObject;  // 활성화할 이펙트 오브젝트 (Inspector에서 연결)
    public float boostDuration = 5f; // 점프력 증가 지속 시간

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TurnOnJumpBoost(boostDuration);

                if (effectObject != null)
                {
                    effectObject.SetActive(true);  // 이펙트 오브젝트 활성화
                }

                Destroy(gameObject);  // 아이템 획득 후 파괴
                Debug.Log("점프력 증가 아이템 획득!");
            }
        }
    }
}