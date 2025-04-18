using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibleItem : MonoBehaviour
{
    public GameObject effectObject;  // 활성화할 이펙트 오브젝트

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TurnOnInvincible();

                if (effectObject != null)
                {
                    effectObject.SetActive(true);
                }
            }
            Destroy(gameObject);
        }
    }
}

