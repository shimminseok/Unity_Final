using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEquipmentCombine : UIBase
{
    [Header("Inventory")]
    [SerializeField] private EquipmentCombineInventoryUI inventoryUI;


    [SerializeField] private List<InventorySlot> materialItemSlotList;
    [SerializeField] private InventorySlot resultItemSlot;

    private CombineManager combineManager;
    private InventoryManager inventoryManager;

    public List<EquipmentItem> CombineItems { get; private set; } = new List<EquipmentItem>();


    private EquipmentItem resultItem;


    private bool IsItemInCombine(EquipmentItem item) => CombineItems.Contains(item);
    private bool CanAddItemToCombine()               => CombineItems.Count < 3;

    private void Start()
    {
        combineManager = CombineManager.Instance;
        inventoryManager = InventoryManager.Instance;

        for (int i = 0; i < materialItemSlotList.Count; i++)
        {
            materialItemSlotList[i].Initialize(null, false);
        }

        resultItemSlot.Initialize(null, false);
    }

    public void ToggleCombineItem(EquipmentItem item)
    {
        if (IsItemInCombine(item))
            RemoveCombineItem(item);
        else if (CanAddItemToCombine())
            AddCombineItem(item);
    }

    private void AddCombineItem(EquipmentItem item)
    {
        if (item.IsEquipped)
        {
            Debug.Log("장착 중인 장비는 합성 할 수 없습니다.");
            return;
        }

        if (CombineItems.Count > 0)
        {
            if (CombineItems[0].ItemSo.Tier != item.ItemSo.Tier)
            {
                Debug.Log("같은 티어의 장비만 합성 할 수 있습니다.");
                return;
            }
        }

        CombineItems.Add(item);
        int emptyIndex = materialItemSlotList.FindIndex(slot => slot.Item == null);
        materialItemSlotList[emptyIndex].Initialize(item, false);
    }

    private void RemoveCombineItem(EquipmentItem item)
    {
        int index = CombineItems.IndexOf(item);
        CombineItems.RemoveAt(index);
        materialItemSlotList[index].Initialize(null, false);
    }

    public void OnClickCombine()
    {
        resultItem = combineManager.TryCombine(CombineItems);
        if (resultItem == null)
            return;

        for (int i = 0; i < CombineItems.Count; i++)
        {
            inventoryManager.RemoveItem(CombineItems[i].Index);
            materialItemSlotList[i].EmptySlot(false);
        }

        inventoryManager.AddItem(resultItem);

        CombineItems.Clear();

        resultItemSlot.Initialize(resultItem, true);
    }


    public override void Close()
    {
        base.Close();
    }

    public override void Open()
    {
        base.Open();
        inventoryUI.Initialize();
    }
}