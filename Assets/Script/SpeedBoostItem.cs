using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostItem : MonoBehaviour
{
    public GameObject effectObject;  // 활성화할 이펙트 오브젝트 (Inspector에서 연결)

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TurnOnSpeedBoost();

                if (effectObject != null)
                {
                    effectObject.SetActive(true);  // 이펙트 활성화
                }
            }
            Destroy(gameObject);
        }
    }
}

