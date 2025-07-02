using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// JSON 파일을 읽어 DialogueGroupSO ScriptableObject를 생성하는 에디터 툴.
/// CSV → JSON → SO 파이프라인의 마지막 단계로 사용된다.
/// </summary>
public static class DialogueJsonImporter
{
    // Unity 메뉴 등록: Tools > Dialogue > Generate DialogueGroupSO From JSON
    [MenuItem("Tools/Dialogue/Generate DialogueGroupSO From JSON")]
    public static void GenerateFromJson()
    {
        // JSON 파일 경로 (CSV에서 변환된 json)
        string jsonPath = Application.dataPath + "/CSV/Dialogue/dialogue_data.json";

        if (!File.Exists(jsonPath))
        {
            Debug.LogError($"JSON 파일이 존재하지 않습니다: {jsonPath}");
            return;
        }

        // JSON 읽기
        string jsonText = File.ReadAllText(jsonPath);
        Debug.Log("[Importer] Raw JSON:\n" + jsonText);

        // JsonUtility는 배열을 직접 못 읽기 때문에 래퍼 사용
        List<DialogueGroupJson> groupList = JsonUtilityWrapper.FromJsonArray<DialogueGroupJson>(jsonText);
        Debug.Log($"[Importer] Parsed Group Count: {groupList?.Count}");

        // 출력 폴더 (ScriptableObject 저장 경로)
        string outputPath = "Assets/10. Tables/Dialogue/";
        if (!Directory.Exists(outputPath))
            Directory.CreateDirectory(outputPath);

        // 각 그룹마다 SO 생성
        foreach (var jsonGroup in groupList)
        {
            Debug.Log($"GroupKey: {jsonGroup.groupKey}, Lines: {jsonGroup.lines?.Count}");
            DialogueGroupSO groupSO = ScriptableObject.CreateInstance<DialogueGroupSO>();
            groupSO.groupKey = jsonGroup.groupKey;
            groupSO.mode = jsonGroup.mode;
            groupSO.lines = jsonGroup.lines;

            string assetPath = $"{outputPath}/{jsonGroup.groupKey}.asset";
            AssetDatabase.CreateAsset(groupSO, assetPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"{groupList.Count}개의 DialogueGroupSO가 JSON으로부터 생성되었습니다.");
    }
}