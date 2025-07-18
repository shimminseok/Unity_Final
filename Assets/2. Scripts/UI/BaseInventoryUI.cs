using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInventoryUI : MonoBehaviour
{
    [SerializeField] protected ReuseScrollview<InventoryItem> reuseScrollview;

    protected InventoryManager InventoryManager => InventoryManager.Instance;
    protected UIManager        UIManager        => UIManager.Instance;

    public Dictionary<int, InventorySlot> InventorySlots { get; private set; } = new();
    protected List<InventorySlot> InventorySlotPool = new List<InventorySlot>();


    protected Action<InventorySlot> OnSlotClicked;
    protected Func<List<InventoryItem>> GetInventorySource;

    public virtual void Initialize(Func<List<InventoryItem>> inventoryGetter, Action<InventorySlot> onClickHandler)
    {
        GetInventorySource = inventoryGetter;
        OnSlotClicked = onClickHandler;

        reuseScrollview.SetData(GetInventorySource());
    }
}