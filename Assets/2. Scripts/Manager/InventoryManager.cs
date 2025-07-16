using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem
{
    public int Index;
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
    public List<InventoryItem> Inventory { get; private set; }


    public event Action<int> OnInventorySlotUpdate;

    private GameManager gameManager;

    public Dictionary<JobType, List<InventoryItem>> JobInventory { get; private set; } = new();

    protected override void Awake()
    {
        base.Awake();
        if (isDuplicated)
            return;

        gameManager = GameManager.Instance;
        InitInventory();
    }

    private void InitInventory()
    {
        Inventory = new List<InventoryItem>(Enumerable.Repeat<InventoryItem>(null, inventorySize));
    }

    public void RemoveItem(int index)
    {
        Inventory[index] = null;
        OnInventorySlotUpdate?.Invoke(index);
    }

    public void AddItem(InventoryItem item, int amount = 1)
    {
        AddNonStackableItem(item, amount);
    }

    /// <summary>
    /// 스택형 아이템을 추가하는 함수
    /// </summary>
    /// <param name="itemSo"></param>
    /// <param name="amount"></param>
    private void AddStackableItem(InventoryItem item, int amount = 1)
    {
        InventoryItem findItem = Inventory.Find(x => x != null && x.ItemSo.ID == item.ItemSo.ID);

        if (findItem == null)
        {
            // To Do 인벤토리가 꽉찼는지 확인
            int index = Inventory.IndexOf(null);
            if (index < 0)
            {
                Debug.Log("인벤토리 공간이 부족합니다.");
                return;
            }
            else
            {
                findItem = new InventoryItem(item.ItemSo, amount);
            }

            Inventory[index] = findItem;
            Inventory[index].Index = index;
            OnInventorySlotUpdate?.Invoke(index);
        }
        else
        {
            findItem.ChangeQuantity(amount);
        }
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
            int index = Inventory.IndexOf(null);
            if (index < 0)
                return;

            var clonedItem = item.Clone();
            Inventory[index] = clonedItem;
            Inventory[index].Index = index;

            if (clonedItem is EquipmentItem equipmentItem)
            {
                if (equipmentItem.EquipmentItemSo.IsEquipableByAllJobs)
                {
                    foreach (JobType job in Enum.GetValues(typeof(JobType)))
                    {
                        AddEquipmentItem(job, equipmentItem);
                    }
                }
                else
                {
                    AddEquipmentItem(equipmentItem.EquipmentItemSo.JobType, equipmentItem);
                }
            }

            OnInventorySlotUpdate?.Invoke(index);
        }
    }

    private void AddEquipmentItem(JobType jobType, EquipmentItem item)
    {
        if (!JobInventory.TryGetValue(jobType, out List<InventoryItem> inventoryList))
        {
            inventoryList = new List<InventoryItem>();
        }

        inventoryList.Add(item);
        JobInventory[jobType] = inventoryList;
    }

    public void SwichItem(int from, int to)
    {
        (Inventory[from], Inventory[to]) = (Inventory[to], Inventory[from]);

        OnInventorySlotUpdate?.Invoke(from);
        OnInventorySlotUpdate?.Invoke(to);
    }
}