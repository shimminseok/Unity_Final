using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EquipmentCombineInventory : MonoBehaviour
{
    [SerializeField] private GameObject inventorySlotPrefab;

    private InventoryManager inventoryManager;
    private CombineManager combineManager;
    private UIManager uiManager;

    public Dictionary<int, InventorySlot> InventorySlots { get; private set; }

    void Start()
    {
    }

    void Update()
    {
    }

    public void Initialize()
    {
        int index = 0;
        foreach (InventoryItem inventoryItem in inventoryManager.Inventory)
        {
            GameObject inventorySlot = Instantiate(inventorySlotPrefab, transform);
            if (inventorySlot.TryGetComponent(out InventorySlot component))
            {
                component.Initialize(index++, inventoryItem);
            }
        }
    }
}