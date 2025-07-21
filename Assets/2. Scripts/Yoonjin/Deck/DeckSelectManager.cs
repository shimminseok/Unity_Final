using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckSelectManager : SceneOnlySingleton<DeckSelectManager>
{
    // 선택된 캐릭터와 스킬 목록
    [SerializeField]
    private List<EntryDeckData> selectedDeck = new List<EntryDeckData>();

    // 최근에 덱에 넣은 캐릭터
    private EntryDeckData currentSelectedCharacter;

    // 최대 선택 가능한 캐릭터 수

    public event Action<int> OnChangedDeck;

    public event Action<EntryDeckData, EquipmentItem, EquipmentItem> OnEquipChanged;

    #region getter

    public List<EntryDeckData> GetSelectedDeck()
    {
        return selectedDeck;
    }

    public EntryDeckData GetCurrentSelectedCharacter()
    {
        return currentSelectedCharacter;
    }

    #endregion

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        selectedDeck = PlayerDeckContainer.Instance.CurrentDeck.DeckDatas;
        if (selectedDeck.Count == 0)
        {
            for (int i = 0; i < Define.MaxCharacterCount; i++)
            {
                selectedDeck.Add(null);
            }
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }


    // 현재 편집 중인 캐릭터
    public void SetCurrentSelectedCharacter(EntryDeckData entry)
    {
        currentSelectedCharacter = entry;
    }

    /// <summary>
    /// 덱에 Unit을 추가하는 메서드
    /// </summary>
    /// <param name="entryDeck"></param>
    public void AddUnitInDeck(EntryDeckData entryDeck, out int index)
    {
        // 새로운 캐릭터 데이터 추가
        index = selectedDeck.IndexOf(null);
        if (index == -1)
            return;

        selectedDeck[index] = entryDeck;
        entryDeck.Compete(true);
        currentSelectedCharacter = entryDeck;
        OnChangedDeck?.Invoke(index);
    }

    /// <summary>
    /// 덱에서 Unit을 제거하는 메서드
    /// </summary>
    /// <param name="entryDeck"></param>
    public void RemoveUnitInDeck(EntryDeckData entryDeck)
    {
        int index = selectedDeck.IndexOf(entryDeck);
        if (index == -1)
            return;
        selectedDeck[index] = null;
        entryDeck.Compete(false);
        currentSelectedCharacter = null;
        OnChangedDeck?.Invoke(index);
    }


    // 캐릭터에 액티브 스킬 장착 & 해제
    public void SelectActiveSkill(ActiveSkillSO activeSkill)
    {
        if (currentSelectedCharacter == null) return;

        var skills = currentSelectedCharacter.skillDatas;

        // 이미 장착된 스킬일 경우 해제
        for (int i = 0; i < skills.Length; i++)
        {
            if (skills[i] == activeSkill)
            {
                skills[i] = null;
                currentSelectedCharacter.InvokeSkillChanged();
                return;
            }
        }

        // 비어 있는 슬롯에 장착
        for (int i = 0; i < skills.Length; i++)
        {
            if (skills[i] == null)
            {
                skills[i] = activeSkill;
                currentSelectedCharacter.InvokeSkillChanged();
                return;
            }
        }
    }

    // 캐릭터에 장비 장착
    public void ProcessEquipSelection(EquipmentItem item)
    {
        if (currentSelectedCharacter == null)
        {
            return;
        }

        // 장비 타입 받아옴

        Dictionary<EquipmentType, EquipmentItem> equipped = currentSelectedCharacter.EquippedItems;

        EquipmentType type = item.EquipmentItemSo.EquipmentType;

        // 현재 type 슬롯에 장착된 아이템
        if (equipped.TryGetValue(type, out var alreadyEquipped))
        {
            // 같은 아이템을 다시 클릭한 경우 해제
            currentSelectedCharacter.UnEquipItem(type);
            if (alreadyEquipped == item)
            {
                OnEquipChanged?.Invoke(currentSelectedCharacter, null, item);
                return;
            }
        }

        // 새 장비 장착
        currentSelectedCharacter.EquipItem(item);
        OnEquipChanged?.Invoke(currentSelectedCharacter, item, alreadyEquipped);
    }

    public void ForceEquipItemToCurrentCharacter(EquipmentItem item)
    {
        if (currentSelectedCharacter == null || item == null || item.EquippedUnit == null)
            return;

        Dictionary<EquipmentType, EquipmentItem> equipped = currentSelectedCharacter.EquippedItems;
        var                                      type     = item.EquipmentItemSo.EquipmentType;

        if (equipped.TryGetValue(type, out var alreadyEquipped))
        {
            currentSelectedCharacter.UnEquipItem(type);
        }

        var fromUnit = item.EquippedUnit;
        fromUnit.UnEquipItem(type);
        currentSelectedCharacter.EquipItem(item);

        OnEquipChanged?.Invoke(currentSelectedCharacter, item, alreadyEquipped);
    }
}