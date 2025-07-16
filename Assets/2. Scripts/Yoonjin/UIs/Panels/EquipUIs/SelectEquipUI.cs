using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SelectEquipUI : UIBase
{
    [Header("장비 슬롯")]
    [SerializeField] private List<InventorySlot> equippedItemsSlot = new();

    [Header("장비 정보 표시")]
    [SerializeField] private TMP_Text itemName;

    [SerializeField] private TMP_Text itemDescription;

    [Header("인벤토리")]
    [SerializeField] private EquipmentUnitInventoryUI inventoryUI;

    public EntryDeckData CurrentCharacter { get; private set; }

    public event Action<EntryDeckData> OnEquipChanged;


    private void Start()
    {
    }

    public override void Open()
    {
        base.Open();
        UpdateEquipUI();

        if (CurrentCharacter == null)
            return;
        AvatarPreviewManager.Instance.ShowAvatar(CurrentCharacter.CharacterSo);
    }

    public void SetCurrentSelectedUnit(EntryDeckData currentUnit)
    {
        DeckSelectManager.Instance.SetCurrentSelectedCharacter(currentUnit);
        CurrentCharacter = currentUnit;
    }

    public override void Close()
    {
        base.Close();
        AvatarPreviewManager.Instance.HideAllAvatars();
        OnEquipChanged?.Invoke(CurrentCharacter);
    }

    // UI 갱신
    public void UpdateEquipUI()
    {
        if (CurrentCharacter == null)
            return;

        ClearEquipInfo();
        equippedItemsSlot.ForEach(slot => slot.Initialize(null, false));
        foreach (var equipmentItem in CurrentCharacter.equippedItems)
        {
            equippedItemsSlot[(int)equipmentItem.Key].Initialize(equipmentItem.Value, false);
            equippedItemsSlot[(int)equipmentItem.Key].ShowEquipMark(false);
        }

        inventoryUI.Initialize();
    }

    public void OnClickInventorySlot(EquipmentItem item)
    {
        if (item.IsEquipped && item.EquippedUnit != CurrentCharacter)
        {
            Debug.Log($"현재 장착된 아이템은 {item.EquippedUnit.CharacterSo.UnitName}이 장착하고 있습니다");
            return;
        }

        DeckSelectManager.Instance.SelectEquipment(item);
        UpdateEquipUI();
    }

    // 장비 정보 텍스트 표시
    private void ShowEquipInfo(EquipmentItem item)
    {
        itemName.text = item.EquipmentItemSo.ItemName;
        itemDescription.text = item.EquipmentItemSo.ItemDescription;
    }

    // 장비 정보 텍스트 삭제
    private void ClearEquipInfo()
    {
        itemName.text = "";
        itemDescription.text = "";
    }
}