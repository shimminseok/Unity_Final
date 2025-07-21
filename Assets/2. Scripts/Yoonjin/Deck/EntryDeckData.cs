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

    public Dictionary<EquipmentType, EquipmentItem> EquippedItems  { get; private set; } = new();
    public int                                      Level          { get; private set; }
    public int                                      Amount         { get; private set; }
    public int                                      TranscendLevel { get; private set; }


    private const int BASE_MAX_LEVEL = 10;
    private const int MAX_TRANSCEND_LEVEL = 5;
    public int MaxLevel => BASE_MAX_LEVEL + (TranscendLevel * BASE_MAX_LEVEL);


    public bool         IsCompeted  { get; private set; }
    public PlayerUnitSO CharacterSo { get; private set; }
    public event Action OnEquipmmmentChanged;
    public event Action OnSkillChanged;
    public event Action OnTranscendChanged;
    public event Action OnLevelUp;


    public EntryDeckData(int id)
    {
        Level = 1;
        Amount = 0;
        TranscendLevel = 0;
        CharacterSo = TableManager.Instance.GetTable<PlayerUnitTable>().GetDataByID(id);
    }

    public void LevelUp(out bool result)
    {
        result = Level < MaxLevel;
        if (result)
        {
            Level++;
            OnLevelUp?.Invoke();
        }
    }

    public void Transcend(out bool result)
    {
        result = TranscendLevel < MAX_TRANSCEND_LEVEL && Amount >= Define.DupeCountByTranscend[TranscendLevel];
        if (result)
        {
            TranscendLevel++;
            OnTranscendChanged?.Invoke();
        }
    }

    public void Compete(bool isCompeted)
    {
        IsCompeted = isCompeted;
    }

    public void AddAmount(int amount = 1)
    {
        Amount += amount;
    }

    public void EquipItem(EquipmentItem item)
    {
        item.EquipItem(this);
        EquippedItems[item.EquipmentItemSo.EquipmentType] = item;
        InvokeEquipmentChanged();
    }

    public void UnEquipItem(EquipmentType type)
    {
        if (EquippedItems.TryGetValue(type, out EquipmentItem item))
        {
            item.UnEquipItem();
            EquippedItems.Remove(type);
            InvokeEquipmentChanged();
        }
    }

    public void InvokeEquipmentChanged()
    {
        OnEquipmmmentChanged?.Invoke();
    }

    public void InvokeSkillChanged()
    {
        OnSkillChanged?.Invoke();
    }
}