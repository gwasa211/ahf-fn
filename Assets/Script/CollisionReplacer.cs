using UnityEngine;

public class CollisionReplacer : MonoBehaviour
{
    [Header("충돌 시 교체 설정")]
    [Tooltip("충돌 시 교체할 플레이어 프리팹")]
    public GameObject replacementPrefab;

    [Tooltip("추가로 소환할 보스 프리팹")]
    public GameObject bossPrefab;

    [Tooltip("교체 대상 태그")]
    public string targetTag = "Player";

    [Header("소환 위치 설정")]
    [Tooltip("보스를 소환할 위치")]
    public Vector3 bossSpawnPositionOffset = new Vector3(3f, 0f, 0f); // 플레이어 기준 오프셋

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
        // 새 플레이어 소환
        if (replacementPrefab != null)
        {
            Instantiate(replacementPrefab, playerPosition, playerRotation);
        }

        // 보스 소환
        if (bossPrefab != null)
        {
            Vector3 bossPosition = playerPosition + bossSpawnPositionOffset;
            Instantiate(bossPrefab, bossPosition, Quaternion.identity);
        }
    }
}
