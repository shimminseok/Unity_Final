using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    [SerializeField] private RectTransform panelRect;
    [SerializeField] private Vector2 onScreenPos;
    [SerializeField] private Vector2 offScreenPos;

    [SerializeField] private StatSlot[] statSlots;

    [SerializeField] private EquipButton[] equipButtons = new EquipButton[3];
    [SerializeField] private SkillSlot[] skillSlots = new SkillSlot[4];
    private UICharacterSetting uiCharacterSetting;

    private EntryDeckData selectedPlayerUnitData;


    private void Awake()
    {
        onScreenPos = panelRect.anchoredPosition;
        offScreenPos = new Vector2(Screen.width, panelRect.anchoredPosition.y);

        panelRect.anchoredPosition = offScreenPos;
    }

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
                float statValue  = stat.Value; //레벨당 증가 스탯도 같이 해주기
                float equipValue = 0;

                foreach (EquipmentItem equipmentItem in selectedPlayerUnitData.equippedItems.Values)
                {
                    var equipStat = equipmentItem.EquipmentItemSo.Stats.Find(s => s.StatType == stat.StatType);
                    if (equipStat != null)
                        equipValue += equipStat.Value;
                }

                statSlots[index++].Initialize(statValue, equipValue);
            }
        }
    }

    private void SetPlayerUnitEquipmentInfo()
    {
        var equipItem = selectedPlayerUnitData.equippedItems;
        for (int i = 0; i < equipButtons.Length; i++)
        {
            if (equipItem.TryGetValue((EquipmentType)i, out EquipmentItem item))
            {
                equipButtons[i].Initialize(item, true, false, null);
            }
            else
            {
                equipButtons[i].Initialize(null, false, false, null);
            }
        }
    }

    private void SetPlayerUnitSkillInfo()
    {
        skillSlots[0].SetSkillIcon(selectedPlayerUnitData.CharacterSo.PassiveSkill);
        int index = 1;
        foreach (ActiveSkillSO activeSkillSo in selectedPlayerUnitData.skillDatas)
        {
            skillSlots[index++].SetSkillIcon(activeSkillSo);
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

        RefreshUI();

        panelRect.DOAnchorPos(onScreenPos, 0.5f).SetEase(Ease.OutCubic);
    }

    public void ClosePanel()
    {
        if (selectedPlayerUnitData != null)
        {
            selectedPlayerUnitData.OnEquipmmmentChanged -= RefreshUI;
            selectedPlayerUnitData.OnSkillChanged -= RefreshUI;
        }

        selectedPlayerUnitData = null;
    }

    private void RefreshUI()
    {
        SetCharacterStatInfo();
        SetPlayerUnitEquipmentInfo();
        SetPlayerUnitSkillInfo();
    }
}