using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class EquipmentCombineInventoryUI : BaseInventoryUI
{
    [SerializeField] private UIEquipmentCombine uiEquipmentCombine;


    private CombineManager combineManager;

    private void Start()
    {
        // Initialize();
    }

    public override void Initialize()
    {
        int index = 0;
        foreach (InventoryItem inventoryItem in InventoryManager.Inventory)
        {
            EquipmentItem equipmentItem = (EquipmentItem)inventoryItem;
            InventorySlot inventorySlot = GetOrCreateInventorySlot(index, InventorySlotPool);

            inventorySlot.OnClickSlot -= uiEquipmentCombine.ToggleCombineItem;
            InventoryManager.OnInventorySlotUpdate -= UpdateInventorySlot;

            inventorySlot.Initialize(equipmentItem, true);
            InventorySlots[index] = inventorySlot;

            inventorySlot.OnClickSlot += uiEquipmentCombine.ToggleCombineItem;
            InventoryManager.OnInventorySlotUpdate += UpdateInventorySlot;
            index++;
        }
    }
}