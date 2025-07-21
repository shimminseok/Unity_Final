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

    private void OnEnable()
    {
        DeckSelectManager.Instance.OnEquipChanged += HandleEquipChanged;
    }

    private void OnDisable()
    {
        DeckSelectManager.Instance.OnEquipChanged -= HandleEquipChanged;
    }

    private void HandleEquipChanged(EntryDeckData unit, EquipmentItem newItem, EquipmentItem oldItem)
    {
        if (unit != CurrentCharacter)
            return;

        RefreshEquipUI();

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
        AvatarPreviewManager.HideAvatar(CurrentCharacter?.CharacterSo);
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
        equippedItemsSlot.ForEach(slot => slot.Initialize(null, false));

        foreach (EquipmentType type in Enum.GetValues(typeof(EquipmentType)))
        {
            if (CurrentCharacter.equippedItems.TryGetValue(type, out EquipmentItem item))
            {
                int slotIndex = (int)type;
                equippedItemsSlot[slotIndex].Initialize(item, false);
                equippedItemsSlot[slotIndex].ShowEquipMark(false);
            }
        }
    }

    private void OnClickInventorySlot(EquipmentItem item)
    {
        if (item.IsEquipped && item.EquippedUnit != CurrentCharacter)
        {
            Debug.Log($"현재 장착된 아이템은 {item.EquippedUnit.CharacterSo.UnitName}이 장착하고 있습니다");
            return;
        }


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
            DeckSelectManager.SelectEquipment(item);
        }
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
    }
}