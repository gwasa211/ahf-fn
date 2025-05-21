using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class RankPage : MonoBehaviour
{
    [SerializeField] Transform contentRoot;   // Content 오브젝트
    [SerializeField] GameObject rowPrefab;    // RankRow 프리팹
    [SerializeField] int targetStage = 1;     // 표시할 스테이지 번호 (인스펙터에서 설정 가능)

    StageResultList allData;

    void Awake()
    {
        allData = StageResultSaver.LoadRank();  // JSON에서 랭킹 로드
        RefreshRankList();
    }

    void RefreshRankList()
    {
        // 기존 자식 오브젝트 삭제
        foreach (Transform child in contentRoot)
        {
            Destroy(child.gameObject);
        }

        // 해당 스테이지의 랭킹 정렬 (플레이어별 최고 점수만 반영)
        var sortedData = allData.results
            .Where(r => r.stage == targetStage)
            .GroupBy(r => r.playerName)
            .Select(g => g.OrderByDescending(x => x.score).First())
            .OrderByDescending(x => x.score)
            .ToList();

        // 랭크 UI 생성
        for (int i = 0; i < sortedData.Count; i++)
        {
            GameObject row = Instantiate(rowPrefab, contentRoot);
            TMP_Text rankText = row.GetComponentInChildren<TMP_Text>();
            rankText.text = $"{i + 1}. {sortedData[i].playerName} - {sortedData[i].score}";
        }
    }
}
