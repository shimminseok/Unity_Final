using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class EquipmentCombineInventory : MonoBehaviour
{
    [SerializeField] private UIEquipmentCombine uiEquipmentCombine;
    [SerializeField] private GameObject inventorySlotPrefab;
    [SerializeField] private Transform inventorySlotParent;

    private InventoryManager inventoryManager;
    private CombineManager combineManager;
    private UIManager uiManager;

    public Dictionary<int, InventorySlot> InventorySlots { get; private set; } = new();

    private void Start()
    {
        inventoryManager = InventoryManager.Instance;
        Initialize();
    }

    private void Initialize()
    {
        int index = 0;
        foreach (InventoryItem inventoryItem in inventoryManager.Inventory)
        {
            EquipmentItem equipmentItem = (EquipmentItem)inventoryItem;
            GameObject    inventorySlot = Instantiate(inventorySlotPrefab, inventorySlotParent);
            if (inventorySlot.TryGetComponent(out InventorySlot component))
            {
                component.Initialize(index, equipmentItem);
                component.OnClickSlot += uiEquipmentCombine.ToggleCombineItem;
                InventorySlots.Add(index, component);
                inventoryManager.OnInventorySlotUpdate += UpdateInventorySlot;

                index++;
            }
        }
    }

    private void UpdateInventorySlot(int index)
    {
        InventorySlots[index].Initialize(index, inventoryManager.Inventory[index] as EquipmentItem);
    }
}