using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// CSV에서 GroupKey별로 DialogueGroupSO를 생성하는 툴
/// </summary>
public static class DialogueGroupGenerator
{
    // 메뉴 경로에 등록
    // [MenuItem("Tools/Dialogue/Generate DialogueGroupSO From CSV")]
    public static void GenerateDialogueGroupsFromCSV()
    {
        // CSV 파일 경로
        string csvPath = Application.dataPath + "/CSV/Dialogue/dialogue_data.csv";

        if (!File.Exists(csvPath))
        {
            Debug.LogError($" CSV 파일이 존재하지 않습니다: {csvPath}");
            return;
        }

        // 읽어온 줄 배열
        string[] lines = File.ReadAllLines(csvPath);

        if (lines.Length <= 1)
        {
            Debug.LogWarning(" CSV에 대사 데이터가 없습니다.");
            return;
        }

        // GroupKey 별로 DialogueLine을 모으는 딕셔너리
        Dictionary<string, List<DialogueLine>> groupedLines = new();

        for (int i = 1; i < lines.Length; i++) // 첫 줄은 헤더
        {
            string[] cols = lines[i].Split(',');

            if (cols.Length < 5) continue;

            DialogueLine line = new DialogueLine()
            {
                groupKey = cols[0].Trim(),
                characterName = cols[1].Trim(),
                dialogue = cols[2].Trim().Replace("\\n", "\n"),
                portraitKey = cols[3].Trim(),
                backgroundKey = cols[4].Trim()
            };

            if (!groupedLines.ContainsKey(line.groupKey))
                groupedLines[line.groupKey] = new List<DialogueLine>();

            groupedLines[line.groupKey].Add(line);
        }

        // 생성된 SO 저장 경로
        string outputPath = "Assets/10. Tables/Dialogue/";
        if (!Directory.Exists(outputPath))
            Directory.CreateDirectory(outputPath);

        // GroupKey별로 SO 생성
        foreach (var kvp in groupedLines)
        {
            string groupKey = kvp.Key;
            List<DialogueLine> linesForGroup = kvp.Value;

            DialogueGroupSO groupSO = ScriptableObject.CreateInstance<DialogueGroupSO>();
            groupSO.groupKey = groupKey;
            groupSO.mode = DialogueMode.Fullscreen; // 기본값. 추후 인스펙터에서 변경 가능
            groupSO.lines = linesForGroup;

            string assetPath = $"{outputPath}/{groupKey}.asset";
            AssetDatabase.CreateAsset(groupSO, assetPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"{groupedLines.Count}개의 DialogueGroupSO 생성 완료");
    }
}