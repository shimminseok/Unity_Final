using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectEquipUI : UIBase
{
    [Header("장비 슬롯")]
    [SerializeField] private List<InventorySlot> equippedItemsSlot = new();

    [Header("장비 정보 표시")]
    [SerializeField] private TextMeshProUGUI itemName;

    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private StatSlot[] itemStatSlots;

    [Header("인벤토리")]
    [SerializeField] private EquipmentUnitInventoryUI inventoryUI;

    public EntryDeckData CurrentCharacter { get; private set; }

    public event Action<EntryDeckData> OnEquipChanged;

    private AvatarPreviewManager AvatarPreviewManager => AvatarPreviewManager.Instance;
    private InventoryManager     InventoryManager     => InventoryManager.Instance;
    private DeckSelectManager    DeckSelectManager    => DeckSelectManager.Instance;

    private InventorySlot selectedItemSlot;


    private void HandleEquipItemChanged(EntryDeckData unit, EquipmentItem newItem, EquipmentItem oldItem)
    {
        if (unit != CurrentCharacter)
            return;

        if (oldItem != null)
            inventoryUI.RefreshAtSlotUI(oldItem);
        if (newItem != null)
            inventoryUI.RefreshAtSlotUI(newItem);
    }

    public override void Open()
    {
        base.Open();

        if (CurrentCharacter == null)
            return;

        RefreshEquipUI();
        DeckSelectManager.Instance.OnEquipItemChanged += HandleEquipItemChanged;
        AvatarPreviewManager.ShowAvatar(CurrentCharacter.CharacterSo);
    }

    public void SetCurrentSelectedUnit(EntryDeckData currentUnit)
    {
        DeckSelectManager.SetCurrentSelectedCharacter(currentUnit);
        CurrentCharacter = currentUnit;
    }

    public override void Close()
    {
        base.Close();
        if (CurrentCharacter != null)
        {
            if (CurrentCharacter.IsCompeted)
            {
                int partyIndex = DeckSelectManager.GetSelectedDeck().FindIndex(c => c.CharacterSo.ID == CurrentCharacter.CharacterSo.ID);
                if (partyIndex != -1)
                    AvatarPreviewManager.ShowAvatar(partyIndex, CurrentCharacter.CharacterSo.JobType);
            }
            else
                AvatarPreviewManager.HideAvatar(CurrentCharacter.CharacterSo);
        }

        DeckSelectManager.Instance.OnEquipItemChanged -= HandleEquipItemChanged;
        OnEquipChanged?.Invoke(CurrentCharacter);
    }

    // UI 갱신
    private void RefreshEquipUI()
    {
        if (CurrentCharacter == null)
            return;

        ClearEquipInfo();
        RefreshEquippedSlots();

        List<InventoryItem> inventoryItems = InventoryManager.GetInventoryItems(CurrentCharacter.CharacterSo.JobType);
        inventoryUI.Initialize(
            () => inventoryItems,
            (slot) =>
            {
                slot.OnClickSlot -= OnClickInventorySlot;
                slot.OnClickSlot += OnClickInventorySlot;
            });
    }

    private void RefreshEquippedSlots()
    {
        foreach (EquipmentType type in Enum.GetValues(typeof(EquipmentType)))
        {
            int slotIndex = (int)type;
            if (CurrentCharacter.EquippedItems.TryGetValue(type, out EquipmentItem item))
            {
                equippedItemsSlot[slotIndex].Initialize(item, false);
                equippedItemsSlot[slotIndex].ShowEquipMark(false);
            }
            else
            {
                equippedItemsSlot[slotIndex].Initialize(null, false);
            }
        }
    }

    private void OnClickInventorySlot(EquipmentItem item)
    {
        inventoryUI.SelectItemSlot(item);
        InventorySlot selectSlot = inventoryUI.GetSlotByItem(item);
        if (selectSlot == null)
            return;


        if (selectedItemSlot != selectSlot)
        {
            SetItemInfoUI(item.EquipmentItemSo);
            selectedItemSlot = selectSlot;
        }
        else
        {
            if (item.IsEquipped && item.EquippedUnit != CurrentCharacter)
            {
                Action leftAction = () =>
                {
                    DeckSelectManager.ForceEquipItemToCurrentCharacter(item);
                    RefreshEquippedSlots();
                };
                string equippedUnitName = item.EquippedUnit.CharacterSo.UnitName;
                string itemName         = item.EquipmentItemSo.ItemName;
                string message          = $"{itemName}은 {equippedUnitName}가 장착 중입니다.\n해제 후 장착하시겠습니까?";
                PopupManager.Instance.GetUIComponent<TwoChoicePopup>()?.SetAndOpenPopupUI("", message, leftAction, null, "장착", "취소");
            }
            else
            {
                DeckSelectManager.ProcessEquipItemSelection(item);
            }
        }

        RefreshEquippedSlots();
    }

    private void SetItemInfoUI(EquipmentItemSO equipmentItem)
    {
        itemName.text = equipmentItem.ItemName;
        itemDescription.text = equipmentItem.ItemDescription;
        int count = Mathf.Min(equipmentItem.Stats.Count, itemStatSlots.Length);

        for (int i = 0; i < itemStatSlots.Length; i++)
        {
            bool isActive = i < count;
            itemStatSlots[i].gameObject.SetActive(isActive);

            if (isActive)
            {
                var stat = equipmentItem.Stats[i];
                itemStatSlots[i].Initialize(stat.StatType, stat.Value);
            }
        }
    }

    // 장비 정보 텍스트 삭제
    private void ClearEquipInfo()
    {
        itemName.text = "";
        itemDescription.text = "";
        foreach (StatSlot itemStatSlot in itemStatSlots)
        {
            itemStatSlot.gameObject.SetActive(false);
        }
    }
}