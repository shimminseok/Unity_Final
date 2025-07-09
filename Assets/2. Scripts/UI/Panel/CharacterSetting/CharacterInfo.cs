using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    [SerializeField] private PlayerUnitIncreaseSo statIncreaseSo;

    [SerializeField] private RectTransform panelRect;
    [SerializeField] private TextMeshProUGUI unitName;
    [SerializeField] private TextMeshProUGUI unitLevel;
    [SerializeField] private StatSlot[] statSlots;

    [SerializeField] private EquipButton[] equipButtons = new EquipButton[3];
    [SerializeField] private SkillSlot[] skillSlots = new SkillSlot[4];

    [Header("UnitLevelUpPanel")]
    [SerializeField] private UnitLevelUpPanel unitLevelUpPanel;


    private UICharacterSetting uiCharacterSetting;
    private Vector2 onScreenPos;
    private Vector2 offScreenPos;
    private EntryDeckData selectedPlayerUnitData;


    private Dictionary<StatType, StatSlot> statSlotDic = new Dictionary<StatType, StatSlot>();

    private void Awake()
    {
        onScreenPos = panelRect.anchoredPosition;
        offScreenPos = new Vector2(Screen.width, panelRect.anchoredPosition.y);

        panelRect.anchoredPosition = offScreenPos;

        InitializeStatSlotDic();
    }

    private void Start()
    {
        uiCharacterSetting = UIManager.Instance.GetUIComponent<UICharacterSetting>();
    }

    private void InitializeStatSlotDic()
    {
        statSlotDic.Clear();

        foreach (var slot in statSlots)
        {
            if (!statSlotDic.TryAdd(slot.StatType, slot))
            {
                Debug.LogWarning($"Duplicate StatSlot for type: {slot.StatType}");
            }
        }
    }

    private void SetCharacterStatInfo()
    {
        var level          = selectedPlayerUnitData.Level;
        var charBaseStats  = selectedPlayerUnitData.CharacterSo.Stats;
        var statGrowthList = statIncreaseSo.Stats;
        var equippedItems  = selectedPlayerUnitData.equippedItems;

        Dictionary<StatType, float> baseStats    = new();
        Dictionary<StatType, float> levelUpStats = new();
        Dictionary<StatType, float> equipStats   = new();

        // 1. 기본 스탯 + 레벨 증가 계산
        foreach (var stat in charBaseStats)
        {
            var statType = stat.StatType;
            baseStats[statType] = stat.Value;

            var growth = statGrowthList.Find(s => s.StatType == statType);
            if (growth != null && level > 1)
                levelUpStats[statType] = growth.Value * (level - 1);
            else
                levelUpStats[statType] = 0;
        }

        // 2. 장비 스탯 누적
        foreach (var equipment in equippedItems.Values)
        {
            foreach (var equipStat in equipment.EquipmentItemSo.Stats)
            {
                equipStats.TryAdd(equipStat.StatType, 0);

                equipStats[equipStat.StatType] += equipStat.Value;
            }
        }

        // 3. StatSlot UI 초기화
        foreach (var statType in statSlotDic.Keys)
        {
            float baseValue  = baseStats.GetValueOrDefault(statType, 0);
            float levelValue = levelUpStats.GetValueOrDefault(statType, 0);
            float equipValue = equipStats.GetValueOrDefault(statType, 0);

            statSlotDic[statType].Initialize(baseValue + levelValue, equipValue);
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

    private void UpdateUnitLevel()
    {
        int level  = selectedPlayerUnitData.Level;
        var unitSo = selectedPlayerUnitData.CharacterSo;

        unitLevel.text = $"Lv. {level}";

        foreach (StatData statData in statIncreaseSo.Stats)
        {
            float baseValue      = unitSo.GetStat(statData.StatType)?.Value ?? 0;
            float increasedValue = baseValue + statData.Value * (level - 1);

            UpdateLevelUpStatValue(statData.StatType, increasedValue);
        }
    }

    public void OpenPanel(EntryDeckData unitData)
    {
        if (selectedPlayerUnitData != null)
        {
            selectedPlayerUnitData.OnEquipmmmentChanged -= RefreshUI;
            selectedPlayerUnitData.OnSkillChanged -= SetPlayerUnitSkillInfo;
        }

        selectedPlayerUnitData = unitData;
        selectedPlayerUnitData.OnEquipmmmentChanged += RefreshUI;
        selectedPlayerUnitData.OnSkillChanged += SetPlayerUnitSkillInfo;

        unitName.text = selectedPlayerUnitData.CharacterSo.UnitName;

        RefreshUI();
        SetPlayerUnitSkillInfo();
        panelRect.DOAnchorPos(onScreenPos, 0.5f).SetEase(Ease.OutCubic);
    }

    public void ClosePanel()
    {
        if (selectedPlayerUnitData != null)
        {
            selectedPlayerUnitData.OnEquipmmmentChanged -= RefreshUI;
            selectedPlayerUnitData.OnSkillChanged -= SetPlayerUnitSkillInfo;
        }

        selectedPlayerUnitData = null;
    }

    private void RefreshUI()
    {
        SetCharacterStatInfo();
        UpdateUnitLevel();
        SetPlayerUnitEquipmentInfo();
    }


    public void OpenUnitLevelUpPanel()
    {
        if (selectedPlayerUnitData != null)
            selectedPlayerUnitData.OnLevelUp -= UpdateUnitLevel;

        selectedPlayerUnitData.OnLevelUp += UpdateUnitLevel;
        unitLevelUpPanel.OpenPanel(selectedPlayerUnitData);
    }


    private void UpdateLevelUpStatValue(StatType statType, float value)
    {
        if (statSlotDic.TryGetValue(statType, out StatSlot statSlot))
            statSlot.UpdateStatValue(value);
    }
}