using System.IO;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AddressableAssets;
#endif
using UnityEngine;

public class LoadDataEnum
{
#if UNITY_EDITOR
    [MenuItem("Tools/Generate SFXName Enum From Group")]
    public static void GenerateSFXEnumFromGroup()
    {
        string groupName = "SFX"; // 생성 기준이 되는 그룹 이름
        var settings = AddressableAssetSettingsDefaultObject.Settings;

        var group = settings.FindGroup(groupName);
        if (group == null)
        {
            Debug.LogError($"그룹 '{groupName}'을(를) 찾을 수 없습니다.");
            return;
        }

        var entries = group.entries;
        if (entries == null || entries.Count == 0)
        {
            Debug.LogWarning($"그룹 '{groupName}'에 에셋이 없습니다.");
            return;
        }

        string enumPath = "Assets/2. Scripts/Suho/SFXName.cs";
        Directory.CreateDirectory(Path.GetDirectoryName(enumPath));

        using (StreamWriter writer = new StreamWriter(enumPath))
        {
            writer.WriteLine("public enum SFXName");
            writer.WriteLine("{");

            foreach (var entry in entries)
            {
                string address = entry.address;
                string enumName = SanitizeEnumName(address);
                writer.WriteLine($"    {enumName},");
            }

            writer.WriteLine("}");
        }

        AssetDatabase.Refresh();
        Debug.Log("SFXName enum 자동 생성 완료 (그룹 기준)");
    }


    [MenuItem("Tools/Generate BGMName Enum From Group")]
    public static void GenerateBGMEnumFromGroup()
    {
        string groupName = "BGM"; // 생성 기준이 되는 그룹 이름
        var settings = AddressableAssetSettingsDefaultObject.Settings;

        var group = settings.FindGroup(groupName);
        if (group == null)
        {
            Debug.LogError($"그룹 '{groupName}'을(를) 찾을 수 없습니다.");
            return;
        }

        var entries = group.entries;
        if (entries == null || entries.Count == 0)
        {
            Debug.LogWarning($"그룹 '{groupName}'에 에셋이 없습니다.");
            return;
        }

        string enumPath = "Assets/2. Scripts/Suho/BGMName.cs";
        Directory.CreateDirectory(Path.GetDirectoryName(enumPath));

        using (StreamWriter writer = new StreamWriter(enumPath))
        {
            writer.WriteLine("public enum SFXName");
            writer.WriteLine("{");

            foreach (var entry in entries)
            {
                string address = entry.address;
                string enumName = SanitizeEnumName(address);
                writer.WriteLine($"    {enumName},");
            }

            writer.WriteLine("}");
        }

        AssetDatabase.Refresh();
        Debug.Log("BGMName enum 자동 생성 완료 (그룹 기준)");
    }

    // 주소를 enum으로 안전하게 변환 (공백 제거, 숫자 시작 등 처리)
    private static string SanitizeEnumName(string address)
    {
        string name = Path.GetFileNameWithoutExtension(address)
            .Replace(" ", "_")
            .Replace("-", "_");

        if (char.IsDigit(name[0]))
            name = "_" + name;

        return name;
    }
#endif



    

    
    
}