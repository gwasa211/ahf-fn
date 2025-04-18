using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneEdge : MonoBehaviour
{
    // 솟아오를 오브젝트
    public GameObject risingObject;

    // 솟아오른 후 위치를 고정할 대상 오브젝트
    public Transform targetPositionObject;

    // 솟아오르는 속도
    public float riseSpeed = 2f;

    // 솟아오르기 최대 높이 (초기 y 위치로부터 얼마나 올라갈지)
    public float maxRiseHeight = 3f;

    // 내부 변수
    private bool isRising = false;
    private Vector3 originalPosition;
    private float targetY;

    void Start()
    {
        if (risingObject != null)
        {
            originalPosition = risingObject.transform.position;
            targetY = originalPosition.y + maxRiseHeight;
        }
    }

    void Update()
    {
        if (isRising && risingObject != null)
        {
            Vector3 pos = risingObject.transform.position;
            pos.y += riseSpeed * Time.deltaTime;

            if (pos.y >= targetY)
            {
                pos = new Vector3(targetPositionObject.position.x, targetPositionObject.position.y, targetPositionObject.position.z);
                risingObject.transform.position = pos;

                // 솟아오르기 끝, 위치 고정
                isRising = false;
            }
            else
            {
                risingObject.transform.position = pos;
            }
        }

        if (isRising && risingObject != null)
        {
            Vector3 currentPos = risingObject.transform.position;
            Vector3 targetPos = new Vector3(targetPositionObject.position.x, targetPositionObject.position.y, targetPositionObject.position.z);

            // 위로 솟아오르는 중이라면 y축만 점진 이동
            if (currentPos.y < targetY)
            {
                currentPos.y += riseSpeed * Time.deltaTime;
                if (currentPos.y > targetY)
                    currentPos.y = targetY;

                risingObject.transform.position = currentPos;
            }
            else
            {
                // 최대 높이에 도달하면 targetPositionObject 위치로 부드럽게 이동
                risingObject.transform.position = Vector3.MoveTowards(currentPos, targetPos, riseSpeed * Time.deltaTime);

                // 거의 도착했으면 위치 고정 및 상승 종료
                if (Vector3.Distance(risingObject.transform.position, targetPos) < 0.01f)
                {
                    risingObject.transform.position = targetPos;
                    isRising = false;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어 태그가 "Player"라고 가정
        if (other.CompareTag("Player"))
        {
            if (risingObject != null && targetPositionObject != null)
            {
                isRising = true;
            }
        }
    }
}
