using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneEdge : MonoBehaviour
{
    public GameObject targetObject; // 솟아오를 오브젝트
    public float riseHeight = 2f;   // 솟아오를 높이
    public float riseSpeed = 2f;    // 솟아오르는 속도

    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private bool isRising = false;
    private bool isFixed = false;

    void Start()
    {
        if (targetObject != null)
        {
            initialPosition = targetObject.transform.position;
            targetPosition = initialPosition + Vector3.up * riseHeight;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // 충돌한 오브젝트가 "Cube" 태그를 가진 경우 실행
        if (collision.gameObject.CompareTag("Player") && !isRising && !isFixed)
        {
            isRising = true;
        }
    }

    void Update()
    {
        if (isRising && !isFixed)
        {
            // targetObject를 위로 이동
            targetObject.transform.position = Vector3.MoveTowards(targetObject.transform.position, targetPosition, riseSpeed * Time.deltaTime);

            // 목표 위치 도달 시 고정
            if (Vector3.Distance(targetObject.transform.position, targetPosition) < 0.01f)
            {
                isRising = false;
                isFixed = true;
            }
        }
    }
}