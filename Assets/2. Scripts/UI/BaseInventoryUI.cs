using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInventoryUI : MonoBehaviour
{
    [SerializeField] protected ReuseScrollview<InventoryItem> reuseScrollview;

    protected InventoryManager InventoryManager => InventoryManager.Instance;
    protected UIManager        UIManager        => UIManager.Instance;

    protected Action<InventorySlot> OnSlotClicked;
    protected Func<List<InventoryItem>> GetInventorySource;
    protected Dictionary<EquipmentItem, InventorySlot> ItemToSlotDic { get; private set; } = new();

    public virtual void Initialize(Func<List<InventoryItem>> inventoryGetter, Action<InventorySlot> onClickHandler)
    {
        GetInventorySource = inventoryGetter;

        reuseScrollview.SetData(GetInventorySource());
        ItemToSlotDic.Clear();
        var dataList = GetInventorySource();
        for (int i = 0; i < reuseScrollview.ItemList.Count; i++)
        {
            if (reuseScrollview.ItemList[i].TryGetComponent<InventorySlot>(out var slot))
            {
                slot.SetOnClickCallback(onClickHandler);

                if (dataList[i] is EquipmentItem item)
                {
                    ItemToSlotDic.Add(item, slot);
                }
            }
        }
    }


    public void RefreshAtSlotUI(InventoryItem item)
    {
        int index = reuseScrollview.GetDataIndexFromItem(item);
        reuseScrollview.RefreshSlotAt(index);
    }
}