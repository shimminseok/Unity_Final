using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInventoryUI : MonoBehaviour
{
    [SerializeField] protected ReuseScrollview<InventoryItem> reuseScrollview;

    protected InventoryManager InventoryManager => InventoryManager.Instance;
    protected UIManager        UIManager        => UIManager.Instance;

    protected Func<List<InventoryItem>> GetInventorySource;
    protected Dictionary<EquipmentItem, InventorySlot> itemToSlotMap { get; private set; } = new();

    public virtual void Initialize(Func<List<InventoryItem>> inventoryGetter, Action<InventorySlot> onClickHandler)
    {
        GetInventorySource = inventoryGetter;

        reuseScrollview.SetData(GetInventorySource());
        itemToSlotMap.Clear();
        List<InventoryItem> dataList = GetInventorySource();

        int count = Mathf.Min(reuseScrollview.ItemList.Count, dataList.Count);

        for (int i = 0; i < count; i++)
        {
            if (reuseScrollview.ItemList[i].TryGetComponent<InventorySlot>(out InventorySlot slot))
            {
                slot.SetOnClickCallback(onClickHandler);
                if (dataList[i] is EquipmentItem item)
                {
                    itemToSlotMap.Add(item, slot);
                }
            }
        }
    }

    public ScrollData<InventoryItem> GetDataByItem(InventoryItem item)
    {
        return reuseScrollview.GetDataFromItem(item);
    }

    public void RefreshAtSlotUI(InventoryItem item)
    {
        int index = reuseScrollview.GetDataIndexFromItem(item);
        reuseScrollview.RefreshSlotAt(index);
    }
}