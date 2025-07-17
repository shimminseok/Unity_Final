using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInventoryUI : MonoBehaviour
{
    [SerializeField] protected InventorySlot inventorySlotPrefab;
    [SerializeField] protected Transform inventorySlotParent;

    protected InventoryManager InventoryManager => InventoryManager.Instance;
    protected UIManager        UIManager        => UIManager.Instance;

    public Dictionary<int, InventorySlot> InventorySlots { get; private set; } = new();
    protected List<InventorySlot> InventorySlotPool = new List<InventorySlot>();


    public abstract void Initialize();


    protected void UpdateInventorySlot(int index)
    {
        if (InventorySlots.TryGetValue(index, out InventorySlot slot))
        {
            slot.Initialize(InventoryManager.Instance.Inventory[index] as EquipmentItem, true);
        }
    }


    protected InventorySlot GetOrCreateInventorySlot(int index, List<InventorySlot> pool)
    {
        InventorySlot slot;

        if (index < pool.Count)
        {
            slot = pool[index];
        }
        else
        {
            slot = Instantiate(inventorySlotPrefab, inventorySlotParent);
            pool.Add(slot);
        }

        return slot;
    }

    protected void DisableRemainingSlots(int fromIndex, List<InventorySlot> pool)
    {
        for (int i = fromIndex; i < pool.Count; i++)
        {
            pool[i].EmptySlot(true);
        }
    }

    private void OnDestroy()
    {
        if (InventoryManager != null)
            InventoryManager.OnInventorySlotUpdate -= UpdateInventorySlot;
    }
}