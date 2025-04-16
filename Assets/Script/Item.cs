using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // �÷��̾� ��ũ��Ʈ ��������
            PlayerController2 player = collision.GetComponent<PlayerController2>();
            if (player != null)
            {
                player.TurnOnSwitch(); // ����ġ �ѱ�
            }

            Destroy(gameObject); // ������ �ı�
        }
    }
}
