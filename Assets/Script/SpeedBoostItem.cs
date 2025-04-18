using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostItem : MonoBehaviour
{
    public GameObject effectObject;  // Ȱ��ȭ�� ����Ʈ ������Ʈ (Inspector���� ����)

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
                    effectObject.SetActive(true);  // ����Ʈ Ȱ��ȭ
                }
            }
            Destroy(gameObject);
        }
    }
}

