using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

/// <summary>
/// CSV 파일을 읽고, 각 파일마다 DialogueGroupSO로 변환해 저장하는 자동화 툴
/// </summary>
public static class DialogueGroupGenerator
{
    // 메뉴에서 호출할 수 있도록 설정
    [MenuItem("Tools/Dialogue/Generate All DialogueGroups From CSVs")]
    public static void GenerateAllDialogueGroups()
    {
        // 1. CSV 파일들이 위치한 경로 설정
        string csvFolderPath = Application.dataPath + "/CSV/Dialogue";

        // 2. 생성된 SO를 저장할 경로 설정
        string outputFolder = "Assets/10. Tables/Dialogues";
        if (!Directory.Exists(outputFolder))
            Directory.CreateDirectory(outputFolder);

        // 3. 폴더 내의 모든 .csv 파일 가져오기
        string[] csvFiles = Directory.GetFiles(csvFolderPath, "*.csv");

        foreach (string filePath in csvFiles)
        {
            // 4. 파일명에서 확장자 제거 → groupKey로 사용
            string fileName = Path.GetFileNameWithoutExtension(filePath);

            // 5. 파일 내용 읽기
            string[] lines = File.ReadAllLines(filePath);

            if (lines.Length <= 1) continue; // 헤더만 있고 내용 없으면 무시

            // 6. 새 DialogueGroupSO 생성
            DialogueGroupSO group = ScriptableObject.CreateInstance<DialogueGroupSO>();
            group.groupKey = fileName;
            group.mode = DialogueMode.Fullscreen; // 기본값. 인스펙터에서 변경 가능

            // 첫 줄은 헤더니까 건너뜀
            for (int i = 1; i < lines.Length; i++) 
            {
                string[] cols = lines[i].Split(',');

                if (cols.Length < 5) continue;

                DialogueLine line = new DialogueLine()
                {
                    characterName = cols[1].Trim(),
                    dialogue = cols[2].Trim().Replace("\\n", "\n"),
                    portraitKey = cols[3].Trim(),
                    backgroundKey = cols[4].Trim()
                };

                group.lines.Add(line);
            }

            // 7. ScriptableObject로 저장
            string assetPath = $"{outputFolder}/{fileName}.asset";
            AssetDatabase.CreateAsset(group, assetPath);
        }

        // 8. 에디터 리프레시
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("모든 DialogueGroupSO 생성 완료");
    }
}