using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kjjkh : MonoBehaviour
{
    public GameObject effectObject;  // Ȱ��ȭ�� ����Ʈ ������Ʈ

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController2 player = collision.GetComponent<PlayerController2>();
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