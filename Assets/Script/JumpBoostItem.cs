using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBoostItem : MonoBehaviour
{
    public GameObject effectObject;  // Ȱ��ȭ�� ����Ʈ ������Ʈ (Inspector���� ����)
    public float boostDuration = 5f; // ������ ���� ���� �ð�

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
                    effectObject.SetActive(true);  // ����Ʈ ������Ʈ Ȱ��ȭ
                }

                Destroy(gameObject);  // ������ ȹ�� �� �ı�
                Debug.Log("������ ���� ������ ȹ��!");
            }
        }
    }
}