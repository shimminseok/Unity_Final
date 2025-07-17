using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentUnitInventoryUI : BaseInventoryUI
{
    [SerializeField] private SelectEquipUI selectEquipUI;

    private JobType selectedUnitJobType;


    public override void Initialize()
    {
        int index = 0;
        foreach (InventoryItem inventoryItem in InventoryManager.JobInventory[selectEquipUI.CurrentCharacter.CharacterSo.JobType])
        {
            EquipmentItem equipmentItem = (EquipmentItem)inventoryItem;
            InventorySlot inventorySlot = GetOrCreateInventorySlot(index, InventorySlotPool);

            inventorySlot.OnClickSlot -= selectEquipUI.OnClickInventorySlot;
            InventoryManager.OnInventorySlotUpdate -= UpdateInventorySlot;


            inventorySlot.Initialize(equipmentItem, true);
            InventorySlots[index] = inventorySlot;

            inventorySlot.OnClickSlot += selectEquipUI.OnClickInventorySlot;
            InventoryManager.OnInventorySlotUpdate += UpdateInventorySlot;
            index++;
        }

        DisableRemainingSlots(index, InventorySlotPool);
    }
}