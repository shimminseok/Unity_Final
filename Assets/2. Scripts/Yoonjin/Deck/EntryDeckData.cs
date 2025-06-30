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
    public PlayerUnitSO characterSO;  

    public ActiveSkillSO[] skillDatas = new ActiveSkillSO[3];
    public PassiveSO passiveSkill;

    // 딕셔너리로 줘!!! 
    public Dictionary<EquipmentType, EquipmentItem> equippedItems = new();

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
            debugEquippedItems.Add(new EquipmentEntry
            {
                type = kvp.Key,
                item = kvp.Value
            });
        }
    }
}
