using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class StageResult
{
    public string playerName;
    public int stage;
    public int score;
}

[System.Serializable]
public class StageResultList
{
    public List<StageResult> results = new List<StageResult>();
}

public static class StageResultSaver
{
    private const string FILE = "stage_results.json";
    private const string PLAYER_NAME = "PlayerName"; // PlayerPrefs 키
    private static string filePath = Path.Combine(Application.persistentDataPath, FILE);

    public static void SaveStage(int stage, int score)
    {
        StageResultList list = LoadInternal();

        string playerName = PlayerPrefs.GetString(PLAYER_NAME, "");
        StageResult entry = new StageResult
        {
            playerName = playerName,
            stage = stage,
            score = score
        };

        // 기존 결과가 있으면 업데이트
        StageResult existing = list.results.Find(r => r.playerName == playerName && r.stage == stage);
        if (existing != null)
        {
            existing.score = score;
        }
        else
        {
            list.results.Add(entry);
        }

        // 점수 높은 순으로 정렬
        list.results.Sort((a, b) => b.score.CompareTo(a.score));

        // JSON 저장
        string json = JsonUtility.ToJson(list, true);
        File.WriteAllText(filePath, json);
    }

    public static StageResultList LoadRank()
    {
        return LoadInternal();
    }
    private static StageResultList LoadInternal()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            StageResultList list = JsonUtility.FromJson<StageResultList>(json);
            if (list != null)
                return list;
        }

        return new StageResultList();
    }
}
