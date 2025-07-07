using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    [SerializeField] StatSlot[] statSlots;

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
            if (index >= statSlots.Length) break;
            if (statSlots[index].StatType == stat.StatType)
            {
                statSlots[index++].Initialize(stat);
            }
        }
    }

    public void OpenPanel(EntryDeckData unitData)
    {
        selectedPlayerUnitData = unitData;
        SetCharacterStatInfo();
    }

    public void ClosePanel()
    {
    }
}