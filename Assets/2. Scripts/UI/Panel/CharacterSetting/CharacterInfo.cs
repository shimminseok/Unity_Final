using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    [SerializeField] private StatSlot[] statSlots;

    [SerializeField] private EquipButton[] equipButtons = new EquipButton[3];
    private UICharacterSetting uiCharacterSetting;

    private EntryDeckData selectedPlayerUnitData;

    private void Start()
    {
        uiCharacterSetting = UIManager.Instance.GetUIComponent<UICharacterSetting>();
    }


    private void SetCharacterStatInfo()
    {
        //TODO : 캐릭터 능력치, 기본 능력치 * 레벨업 능력치, + 장비 장착
        int index = 0;
        foreach (StatData stat in selectedPlayerUnitData.CharacterSo.Stats)
        {
            if (index >= statSlots.Length)
                break;
            if (statSlots[index].StatType == stat.StatType)
            {
                statSlots[index++].Initialize(stat);
            }
        }
    }

    private void SetCharacterEquipmentInfo()
    {
        foreach (KeyValuePair<EquipmentType, EquipmentItem> equipmentItem in selectedPlayerUnitData.equippedItems)
        {
            equipButtons[(int)equipmentItem.Key].Initialize(equipmentItem.Value, true, false, null);
        }
    }

    public void OpenPanel(EntryDeckData unitData)
    {
        if (selectedPlayerUnitData != null)
        {
            selectedPlayerUnitData.OnEquipmmmentChanged -= RefreshUI;
            selectedPlayerUnitData.OnSkillChanged -= RefreshUI;
        }

        selectedPlayerUnitData = unitData;
        selectedPlayerUnitData.OnEquipmmmentChanged += RefreshUI;
        selectedPlayerUnitData.OnSkillChanged += RefreshUI;
        SetCharacterStatInfo();
        SetCharacterEquipmentInfo();
    }

    public void ClosePanel()
    {
    }

    private void RefreshUI()
    {
        SetCharacterStatInfo();
        SetCharacterEquipmentInfo();
    }
}