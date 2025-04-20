using UnityEngine;

public class CollisionReplacer : MonoBehaviour
{
    [Header("�浹 �� ��ü ����")]
    [Tooltip("�浹 �� ��ü�� �÷��̾� ������")]
    public GameObject replacementPrefab;

    [Tooltip("�߰��� ��ȯ�� ���� ������")]
    public GameObject bossPrefab;

    [Tooltip("��ü ��� �±�")]
    public string targetTag = "Player";

    [Header("��ȯ ��ġ ����")]
    [Tooltip("������ ��ȯ�� ��ġ")]
    public Vector3 bossSpawnPositionOffset = new Vector3(3f, 0f, 0f); // �÷��̾� ���� ������

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(targetTag))
        {
            ReplaceAndSummonBoss(collision.transform.position, collision.transform.rotation);
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            ReplaceAndSummonBoss(other.transform.position, other.transform.rotation);
            Destroy(other.gameObject);
        }
    }

    private void ReplaceAndSummonBoss(Vector3 playerPosition, Quaternion playerRotation)
    {
        // �� �÷��̾� ��ȯ
        if (replacementPrefab != null)
        {
            Instantiate(replacementPrefab, playerPosition, playerRotation);
        }

        // ���� ��ȯ
        if (bossPrefab != null)
        {
            Vector3 bossPosition = playerPosition + bossSpawnPositionOffset;
            Instantiate(bossPrefab, bossPosition, Quaternion.identity);
        }
    }
}
