using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemTrigger : MonoBehaviour
{
    public GameObject itemPrefab; // 떨어질 물체의 프리팹
    public Transform dropPoint; // 물체가 떨어질 위치

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // 캐릭터가 트리거에 들어왔을 때
        {
            DropItem();
        }
    }

    private void DropItem()
    {
        Instantiate(itemPrefab, dropPoint.position, Quaternion.identity); // 물체 생성
    }
}
