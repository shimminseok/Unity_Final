using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentUnitInventoryUI : BaseInventoryUI
{
    [SerializeField] private SelectEquipUI selectEquipUI;

    private JobType selectedUnitJobType;

    public InventorySlot GetSlotByItem(EquipmentItem item)
    {
        return ItemToSlotDic[item];
    }
}