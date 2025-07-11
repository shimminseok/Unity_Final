using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineManager : SceneOnlySingleton<CombineManager>
{
    public event Action<InventoryItem> OnItemCombined;


    public InventoryItem TryCombine(List<InventoryItem> items)
    {
        if (items.Count != 3)
        {
            return null;
        }

        //다음 등급에서 같은 종류의 아이템을 가져와야함.
        // var newItem = new InventoryItem();
        // return 

        return null;
    }
}