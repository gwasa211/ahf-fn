using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneEdge : MonoBehaviour
{
    // �ھƿ��� ������Ʈ
    public GameObject risingObject;

    // �ھƿ��� �� ��ġ�� ������ ��� ������Ʈ
    public Transform targetPositionObject;

    // �ھƿ����� �ӵ�
    public float riseSpeed = 2f;

    // �ھƿ����� �ִ� ���� (�ʱ� y ��ġ�κ��� �󸶳� �ö���)
    public float maxRiseHeight = 3f;

    // ���� ����
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

                // �ھƿ����� ��, ��ġ ����
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

            // ���� �ھƿ����� ���̶�� y�ุ ���� �̵�
            if (currentPos.y < targetY)
            {
                currentPos.y += riseSpeed * Time.deltaTime;
                if (currentPos.y > targetY)
                    currentPos.y = targetY;

                risingObject.transform.position = currentPos;
            }
            else
            {
                // �ִ� ���̿� �����ϸ� targetPositionObject ��ġ�� �ε巴�� �̵�
                risingObject.transform.position = Vector3.MoveTowards(currentPos, targetPos, riseSpeed * Time.deltaTime);

                // ���� ���������� ��ġ ���� �� ��� ����
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
        // �÷��̾� �±װ� "Player"��� ����
        if (other.CompareTag("Player"))
        {
            if (risingObject != null && targetPositionObject != null)
            {
                isRising = true;
            }
        }
    }
}
