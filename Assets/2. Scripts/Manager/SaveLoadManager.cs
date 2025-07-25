using UnityEngine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

public class SaveLoadManager : Singleton<SaveLoadManager>
{
    private static readonly string SavePath = Application.persistentDataPath + "/savedata.json";
    public SaveData SaveData { get; private set; } = new();

    public void Save()
    {
        string jsonData = JsonConvert.SerializeObject(SaveData, Formatting.Indented);
        File.WriteAllText(SavePath, jsonData);
    }

    public SaveData Load()
    {
        if (!File.Exists(SavePath))
        {
            Debug.Log("Save file not found");
            return new SaveData();
        }

        string   json = File.ReadAllText(SavePath);
        SaveData data = JsonConvert.DeserializeObject<SaveData>(json);
        Debug.Log("로드 완료");
        return data;
    }
}

[Serializable]
public class SaveData
{
    /*
     * 세이브 목록
     * 1. BestStage
     * 2. CurrentStage
     * 3. 현재 보유 캐릭터
     * 4. 현재 보유 아이템
     * 5. 현재 진행된 튜토리얼
     * 6. 덱 빌딩 (장착한 아이템, 장착한 스킬)
     * 7. 재화 (Gold, Opal)
     */
    public int BestStage    { get; set; }
    public int CurrentStage { get; set; }
    public int Gold         { get; set; }
    public int Opal         { get; set; }
    public int Tutorial     { get; set; }

    [JsonProperty]
    public Dictionary<int, SaveInventoryItem> InventoryItems { get; set; } = new();

    public void UpdateGold(int gold)
    {
        Gold = gold;
    }

    public void UpdateOpal(int opal)
    {
        Opal = opal;
    }

    public void UpdateBestStage(int bestStage)
    {
        BestStage = bestStage;
    }

    public void UpdateCurrentStage(int currentStage)
    {
        CurrentStage = currentStage;
    }

    public void UpdateInventoryItem(InventoryItem data)
    {
        if (!InventoryItems.TryGetValue(data.ItemSo.ID, out SaveInventoryItem item))
        {
            item = new SaveInventoryItem(data);
            InventoryItems.Add(data.ItemSo.ID, item);
        }

        item.Quantity += 1;
    }
}