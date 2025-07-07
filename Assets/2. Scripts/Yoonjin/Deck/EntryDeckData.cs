using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EntryDeckData
{
    // 캐릭터 직업, 액티브 스킬 3개, 패시브 스킬
    // 장비 등등 equipItem
    // 패시브SO
    public ActiveSkillSO[] skillDatas = new ActiveSkillSO[3];
    public PassiveSO passiveSkill;

    public Dictionary<EquipmentType, EquipmentItem> equippedItems = new();
    public int Level          { get; private set; }
    public int Amount         { get; private set; }
    public int TranscendLevel { get; private set; }


    private const int BASE_MAX_LEVEL = 10;
    private const int Max_TRANSCEND_LEVEL = 5;
    public int MaxLevel => BASE_MAX_LEVEL + (TranscendLevel * BASE_MAX_LEVEL);

    public PlayerUnitSO CharacterSo { get; private set; }

    public EntryDeckData(int id)
    {
        Level = 1;
        Amount = 1;
        TranscendLevel = 0;
        CharacterSo = TableManager.Instance.GetTable<PlayerUnitTable>().GetDataByID(id);
    }

    public void LevelUp(out bool result)
    {
        result = Level < MaxLevel;
        if (result)
        {
            Level++;
        }
    }

    public void Transcend(out bool result)
    {
        result = TranscendLevel < Max_TRANSCEND_LEVEL;
        if (result)
            TranscendLevel++;
    }

    public void AddAmount(int amount = 1)
    {
        Amount += amount;
    }

    [System.Serializable]
    public class EquipmentEntry
    {
        public EquipmentType type;
        public EquipmentItem item;
    }


    /// <summary>
    /// 딕셔너리 디버깅용
    /// </summary>
    [SerializeField]
    private List<EquipmentEntry> debugEquippedItems = new();

    public void SyncDebugEquipments()
    {
        debugEquippedItems.Clear();

        foreach (var kvp in equippedItems)
        {
            debugEquippedItems.Add(new EquipmentEntry { type = kvp.Key, item = kvp.Value });
        }
    }
}