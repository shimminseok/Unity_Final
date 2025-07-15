using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEquipmentCombine : UIBase
{
    [SerializeField] private List<InventorySlot> materialItemSlotList;
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
            materialItemSlotList[i].Initialize(i, null);
        }
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
        materialItemSlotList[emptyIndex].Initialize(item.Index, item);
    }

    private void RemoveCombineItem(EquipmentItem item)
    {
        int index = CombineItems.IndexOf(item);
        CombineItems.RemoveAt(index);
        materialItemSlotList[index].Initialize(index, null);
    }

    public void OnClickCombine()
    {
        resultItem = combineManager.TryCombine(CombineItems);
        if (resultItem != null)
        {
            for (int i = 0; i < CombineItems.Count; i++)
            {
                inventoryManager.RemoveItem(CombineItems[i].Index);
                materialItemSlotList[i].EmptySlot();
            }

            inventoryManager.AddItem(resultItem);

            CombineItems.Clear();
        }
    }
}