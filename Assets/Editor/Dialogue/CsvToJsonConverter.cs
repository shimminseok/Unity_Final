using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// CSV 대사 파일을 JSON 구조로 변환하는 Unity 에디터 툴
/// </summary>
public static class CsvToJsonConverter
{
    // Unity 메뉴 등록
    [MenuItem("Tools/Dialogue/Convert CSV to JSON")]
    public static void ConvertCsvToJson()
    {
        // CSV 입력 파일 경로
        string csvPath = Application.dataPath + "/CSV/Dialogue/dialogue_data.csv";

        // 변환 결과 저장할 JSON 파일 경로
        string jsonPath = Application.dataPath + "/CSV/Dialogue/dialogue_data.json";

        // 파일 존재 여부 확인
        if (!File.Exists(csvPath))
        {
            Debug.LogError($"CSV 파일이 존재하지 않습니다: {csvPath}");
            return;
        }

        // 전체 라인 읽기
        string[] lines = File.ReadAllLines(csvPath);
        if (lines.Length <= 1)
        {
            Debug.LogWarning("CSV에 대사 데이터가 없습니다.");
            return;
        }

        // GroupKey별로 DialogueGroupJson으로 묶을 딕셔너리
        Dictionary<string, DialogueGroupJson> grouped = new();

        // 헤더 라인 파싱
        string[] headers = ParseCsvLine(lines[0]);

        // 실제 데이터 라인 파싱
        for (int i = 1; i < lines.Length; i++)
        {
            string[] cols = ParseCsvLine(lines[i]);
            if (cols.Length < 5) continue;

            string groupKey = cols[0].Trim();

            DialogueLine line = new DialogueLine
            {
                groupKey = groupKey,
                characterName = cols[1].Trim(),
                dialogue = cols[2].Trim().Replace("\\n", "\n"),
                portraitKey = cols[3].Trim(),
                backgroundKey = cols[4].Trim(),
                portraitLeft = cols.Length > 5 ? cols[5].Trim() : "",
                portraitRight = cols.Length > 6 ? cols[6].Trim() : ""
            };


            // 새 그룹이면 초기화
            if (!grouped.ContainsKey(groupKey))
            {
                grouped[groupKey] = new DialogueGroupJson
                {
                    groupKey = groupKey,
                    mode = DialogueMode.Fullscreen, // 기본 출력 모드
                    lines = new List<DialogueLine>()
                };
            }

            // 해당 그룹에 대사 추가
            grouped[groupKey].lines.Add(line);
        }

        // Dictionary → List 변환
        List<DialogueGroupJson> result = new(grouped.Values);

        // JSON 직렬화 (배열 지원용 래퍼 사용)
        string json = JsonUtility.ToJson(new Wrapper<DialogueGroupJson> { items = result }, true);

        // 결과 저장
        File.WriteAllText(jsonPath, json);

        // Unity 에셋 새로고침
        AssetDatabase.Refresh();
        Debug.Log($"CSV를 JSON으로 변환 완료: {jsonPath}");
    }

    /// <summary>
    /// CSV 한 줄을 쉼표와 따옴표 처리 포함하여 안전하게 파싱
    /// </summary>
    private static string[] ParseCsvLine(string line)
    {
        List<string> fields = new();
        bool inQuotes = false;
        string field = "";

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (c == '"')
            {
                // 따옴표 안의 ""는 이스케이프된 따옴표
                if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                {
                    field += '"';
                    i++;
                }
                else
                {
                    inQuotes = !inQuotes;
                }
            }
            else if (c == ',' && !inQuotes)
            {
                // 쉼표는 필드 분리 기준
                fields.Add(field);
                field = "";
            }
            else
            {
                field += c;
            }
        }

        fields.Add(field);
        return fields.ToArray();
    }

    /// <summary>
    /// JsonUtility는 배열을 직접 처리하지 못하므로 래퍼 클래스를 사용
    /// </summary>
    [System.Serializable]
    private class Wrapper<T>
    {
        public List<T> items;
    }
}