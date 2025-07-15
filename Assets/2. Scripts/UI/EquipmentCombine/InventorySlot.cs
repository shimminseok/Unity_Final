using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    public int Index { get; private set; }


    public InventoryItem Item { get; private set; }

    public void Initialize(int index, InventoryItem item)
    {
        Index = index;
        Item = item;
    }
}