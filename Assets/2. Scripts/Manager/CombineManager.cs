using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class CombineManager : SceneOnlySingleton<CombineManager>
{
    public event Action<InventoryItem> OnItemCombined;


    private readonly ItemTable itemTable = TableManager.Instance.GetTable<ItemTable>();
    private readonly Tier maxTier = Enum.GetValues(typeof(Tier)).Cast<Tier>().Max();

    public EquipmentItem TryCombine(List<EquipmentItem> items)
    {
        if (items.Count != 3 || items.Count == 0)
        {
            return null;
        }


        EquipmentType combineResultType = items[Random.Range(0, items.Count)].EquipmentItemSo.EquipmentType;
        Tier          nextTier          = items[0].EquipmentItemSo.Tier;
        if (items[0].EquipmentItemSo.Tier < maxTier)
        {
            nextTier += 1;
        }

        List<EquipmentItemSO> combineItemList = itemTable.GetEquipmentsByTypeAndTier(combineResultType, nextTier);

        if (combineItemList == null || combineItemList.Count == 0)
            return null;
        EquipmentItemSO combineItemSo = combineItemList[Random.Range(0, combineItemList.Count)];

        EquipmentItem combineItem = new EquipmentItem(combineItemSo);

        return combineItem;
    }
}