using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

// 모든 StageSO에 대해, groupKey 규칙을 기반으로 before/afterDialogueKey를 자동으로 설정해주는 에디터 툴
public class DialogueStageBinder : MonoBehaviour
{
    [MenuItem("Tools/Dialogue/Auto Bind Dialogue to Stages")]

    public static void AutoBind()
    {
        // Assets에서 전체 StageSO 수집
        string[] stageGuids = AssetDatabase.FindAssets("t:StageSO", new[] { "Assets" });
        List<StageSO> stageList = new();

        foreach (var guid in stageGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            StageSO stage = AssetDatabase.LoadAssetAtPath<StageSO>(path);
            if (stage != null) stageList.Add(stage);
        }

        // 모든 DialogueGroupSO 수집(Dialogue 테이블 경로 기준)
        string[] dialogueGuids = AssetDatabase.FindAssets("t:DialogueGroupSO", new[] { "Assets/10. Tables/Dialogue" });
        List<DialogueGroupSO> groupList = new();

        foreach (var guid in dialogueGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            DialogueGroupSO group = AssetDatabase.LoadAssetAtPath<DialogueGroupSO>(path);
            if (group != null) groupList.Add(group);
        }

        // group Key -> StageID로 변환 후, StageSO.ID와 비교
        int matchCount = 0;

        foreach (var stage in stageList)
        {
            string assetPath = AssetDatabase.GetAssetPath(stage);

            foreach (var group in groupList)
            {
                // ex: Stage_1_1_Before -> ID 1010101, Type = "Before"
                if (TryParseKey(group.groupKey, out int targetID, out string type) && targetID == stage.ID)
                {
                    Debug.Log($"✅ Match: {group.groupKey} → ID {targetID}, Type {type}, Stage ID: {stage.ID}");

                    // SerializedObject를 통해 강제 반영
                    SerializedObject so = new SerializedObject(stage);
                    if (type == "BEFORE")
                        so.FindProperty("beforeDialogueKey").stringValue = group.groupKey;
                    else if (type == "AFTER")
                        so.FindProperty("afterDialogueKey").stringValue = group.groupKey;
                    so.ApplyModifiedProperties();

                    EditorUtility.SetDirty(stage);
                    AssetDatabase.WriteImportSettingsIfDirty(assetPath);
                    AssetDatabase.ImportAsset(assetPath);
                    matchCount++;
                }
                else
                {
                    Debug.Log($"⛔ No Match: {group.groupKey} → Parsed ID: {targetID}, Stage ID: {stage.ID}");
                }
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"🔄 AutoBind 완료. 총 매핑 수: {matchCount}");
    }

    // groupKey를 파싱하여 해당 StageID와 타입(Before/After)을 추출하는 유틸리티
    // ex: "Stage_1_1_Before" -> 1010101 반환
    private static bool TryParseKey(string key, out int stageID, out string type)
    {
        stageID = 0;
        type = "";

        string[] split = key.Split('_');
        if (split.Length != 4 || !split[0].Equals("STAGE", System.StringComparison.OrdinalIgnoreCase)) return false;

        if (!int.TryParse(split[1], out int chapter)) return false;
        if (!int.TryParse(split[2], out int index)) return false;

        type = split[3].ToUpperInvariant();
        if (type != "BEFORE" && type != "AFTER") return false;

        // 규칙 반영: 4010108 ← chapter=4, index=8
        stageID = int.Parse($"{chapter}0101{index:00}");

        return true;
    }



}
