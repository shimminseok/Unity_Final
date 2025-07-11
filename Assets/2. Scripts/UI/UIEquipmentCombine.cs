using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEquipmentCombine : UIBase
{
    private CombineManager combineManager;


    public List<EquipmentItem> CombineItems { get; private set; }

    private void Start()
    {
        combineManager = CombineManager.Instance;
    }

    public void SetCombineItem(EquipmentItem item)
    {
        if (CombineItems.Contains(item))
        {
            CombineItems.Remove(item);
        }
        else
        {
            CombineItems.Add(item);
        }
    }

    public void OnClickCombine()
    {
        combineManager.TryCombine(CombineItems);
    }
}