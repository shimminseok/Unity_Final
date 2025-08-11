using System.IO;
using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class ItemsCSVtoSO : EditorWindow
{
    // CSV 파일 경로
    private static string csvPath = "Assets/CSV/Item/ItemData_KR_v01.csv";

    // Editor Tool에서 실행
    [MenuItem("Tools/Import Items from CSV")]
    public static void ShowWindow()
    {
        GetWindow<ItemsCSVtoSO>("Import Items");
    }

    // 팝업으로 한번 더 확인
    private void OnGUI()
    {
        GUILayout.Label($"{csvPath}를 ItemSO로 변환하겠습니까?", EditorStyles.boldLabel);

        if (GUILayout.Button("변환"))
        {
            ImportCSV(csvPath);
        }
    }

    private static void ImportCSV(string path)
    {
        // 파일 존재 여부 확인
        if (!File.Exists(csvPath))
        {
            Debug.LogError($"CSV 파일이 존재하지 않습니다: {csvPath}");
            return;
        }

        string[] lines = File.ReadAllLines(path);

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(new[] { ',' }, StringSplitOptions.None);

            EquipmentItemSO item = CreateInstance<EquipmentItemSO>();

            item.ID = int.Parse(values[0]);
            if (Enum.TryParse(values[1], out EquipmentType equipmentType))
            {
                item.EquipmentType = equipmentType;
            }

            if (item.EquipmentType == EquipmentType.Weapon && Enum.TryParse(values[2], out JobType jobType))
            {
                item.JobType = jobType;
            }

            item.ItemName = values[4];
            item.ItemDescription = values[5];

            // 타입별 폴더 경로 설정
            string subFolder = values[1];
            if (item.EquipmentType == EquipmentType.Weapon)
            {
                subFolder += $"/{values[2]}";
            }

            // 이미지 연결
            string spriteName       = values[6];
            string spriteBaseFolder = "Assets/99. Assets/Sprite/Icon/Equipment";
            string spritePath       = $"{spriteBaseFolder}/{subFolder}/{spriteName}.png";
            Sprite sprite           = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);

            if (sprite != null)
            {
                item.ItemSprite = sprite;
            }
            else
            {
                Debug.LogWarning($"Sprite not found: {spritePath}");
            }

            if (Enum.TryParse(values[7], out Tier tier))
            {
                item.Tier = tier;
            }

            if (item.EquipmentType == EquipmentType.Weapon)
            {
                item.IsEquipableByAllJobs = false;
            }
            else
            {
                item.IsEquipableByAllJobs = true;
            }

            item.Stats = new List<StatData>();

            if (float.TryParse(values[8], out float attack))
            {
                item.Stats.Add(new StatData
                {
                    StatType = StatType.AttackPow,
                    ModifierType = StatModifierType.Equipment,
                    Value = attack
                });
            }

            if (float.TryParse(values[9], out float maxHp))
            {
                item.Stats.Add(new StatData
                {
                    StatType = StatType.MaxHp,
                    ModifierType = StatModifierType.Equipment,
                    Value = maxHp
                });
            }

            if (float.TryParse(values[10], out float defense))
            {
                item.Stats.Add(new StatData
                {
                    StatType = StatType.Defense,
                    ModifierType = StatModifierType.Equipment,
                    Value = defense
                });
            }

            if (float.TryParse(values[11], out float criticalRate))
            {
                item.Stats.Add(new StatData
                {
                    StatType = StatType.CriticalRate,
                    ModifierType = StatModifierType.Equipment,
                    Value = criticalRate
                });
            }

            if (float.TryParse(values[12], out float criticalDamage))
            {
                item.Stats.Add(new StatData
                {
                    StatType = StatType.CriticalDam,
                    ModifierType = StatModifierType.Equipment,
                    Value = criticalDamage
                });
            }

            if (float.TryParse(values[13], out float speed))
            {
                item.Stats.Add(new StatData
                {
                    StatType = StatType.Speed,
                    ModifierType = StatModifierType.Equipment,
                    Value = speed
                });
            }

            if (float.TryParse(values[14], out float counter))
            {
                item.Stats.Add(new StatData
                {
                    StatType = StatType.Counter,
                    ModifierType = StatModifierType.Equipment,
                    Value = counter
                });
            }

            string soBaseFolder = "Assets/10. Tables/Item/Equipment";
            string assetPath    = $"{soBaseFolder}/{subFolder}/{item.ID}_{values[3]}.asset";
            AssetDatabase.CreateAsset(item, assetPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Equipment items imported successfully!");
    }
}