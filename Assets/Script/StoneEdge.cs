using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneEdge : MonoBehaviour
{
    public GameObject targetObject; // �ھƿ��� ������Ʈ
    public float riseHeight = 2f;   // �ھƿ��� ����
    public float riseSpeed = 2f;    // �ھƿ����� �ӵ�

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
        // �浹�� ������Ʈ�� "Cube" �±׸� ���� ��� ����
        if (collision.gameObject.CompareTag("Player") && !isRising && !isFixed)
        {
            isRising = true;
        }
    }

    void Update()
    {
        if (isRising && !isFixed)
        {
            // targetObject�� ���� �̵�
            targetObject.transform.position = Vector3.MoveTowards(targetObject.transform.position, targetPosition, riseSpeed * Time.deltaTime);

            // ��ǥ ��ġ ���� �� ����
            if (Vector3.Distance(targetObject.transform.position, targetPosition) < 0.01f)
            {
                isRising = false;
                isFixed = true;
            }
        }
    }
}