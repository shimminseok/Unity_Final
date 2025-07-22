using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;

public class InventoryItem
{
    public int InventoryId { get; set; } // 추가
    public ItemSO ItemSo;
    public int Quantity;

    public InventoryItem(ItemSO itemSo, int quantity)
    {
        ItemSo = itemSo;
        Quantity = quantity;
    }

    public event Action OnItemChanged;

    public virtual InventoryItem Clone() => new InventoryItem(ItemSo, Quantity);

    public void ChangeQuantity(int amount)
    {
        Quantity += amount;
        ItemChanged();
    }

    public void ItemChanged()
    {
        OnItemChanged?.Invoke();
    }
}

[Serializable]
public class SaveInventoryItem
{
    public int Id;
    public int Quantity;

    public SaveInventoryItem(InventoryItem item)
    {
        Id = item.ItemSo.ID;
        Quantity = item.Quantity;
    }

    public SaveInventoryItem()
    {
    }
}

public class InventoryManager : Singleton<InventoryManager>
{
    [SerializeField] private int inventorySize;

    public event Action<int> OnInventorySlotUpdate;
    private GameManager gameManager;


    private Dictionary<int, InventoryItem> inventory = new();
    public IReadOnlyDictionary<int, InventoryItem> Inventory    => inventory;
    public Dictionary<JobType, List<int>>          JobInventory { get; private set; } = new();
    private int nextId = 0;

    protected override void Awake()
    {
        base.Awake();
        if (isDuplicated)
            return;

        gameManager = GameManager.Instance;
    }


    public void AddItem(InventoryItem item, int amount = 1)
    {
        AddNonStackableItem(item, amount);
    }

    public void RemoveItem(int id)
    {
        if (!inventory.Remove(id))
            return;

        foreach (var jobList in JobInventory.Values)
        {
            jobList.Remove(id);
        }

        OnInventorySlotUpdate?.Invoke(id);
    }

    /// <summary>
    /// 비스택형 아이템을 추가하는 함수
    /// </summary>
    /// <param name="itemSo"></param>
    /// <param name="amount"></param>
    private void AddNonStackableItem(InventoryItem item, int amount = 1)
    {
        for (int i = 0; i < amount; i++)
        {
            InventoryItem clonedItem = item.Clone();
            clonedItem.InventoryId = nextId;
            inventory[nextId] = clonedItem;

            if (clonedItem is EquipmentItem equipmentItem)
            {
                if (equipmentItem.EquipmentItemSo.IsEquipableByAllJobs)
                {
                    foreach (JobType job in Enum.GetValues(typeof(JobType)))
                    {
                        AddEquipmentItem(job, nextId);
                    }
                }
                else
                {
                    AddEquipmentItem(equipmentItem.EquipmentItemSo.JobType, nextId);
                }
            }

            OnInventorySlotUpdate?.Invoke(nextId);
            nextId++;
        }
    }

    private void AddEquipmentItem(JobType jobType, int itemId)
    {
        if (!JobInventory.TryGetValue(jobType, out List<int> inventoryList))
        {
            inventoryList = new List<int>();
        }

        inventoryList.Add(itemId);
        JobInventory[jobType] = inventoryList;
    }

    public List<InventoryItem> GetInventoryItems(JobType jobType)
    {
        if (!JobInventory.TryGetValue(jobType, out List<int> idList))
            return new List<InventoryItem>();

        var items = idList.Where(id => inventory.ContainsKey(id)).Select(id => inventory[id]).ToList();
        return items;
    }

    public List<InventoryItem> GetInventoryItems()
    {
        return inventory.Values.OrderBy(item => item.InventoryId).ToList();
    }
}