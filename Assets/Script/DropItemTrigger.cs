using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemTrigger : MonoBehaviour
{
    public GameObject itemPrefab; // ������ ��ü�� ������
    public Transform dropPoint; // ��ü�� ������ ��ġ

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // ĳ���Ͱ� Ʈ���ſ� ������ ��
        {
            DropItem();
        }
    }

    private void DropItem()
    {
        Instantiate(itemPrefab, dropPoint.position, Quaternion.identity); // ��ü ����
    }
}
