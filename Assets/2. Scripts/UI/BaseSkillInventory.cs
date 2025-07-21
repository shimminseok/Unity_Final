using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseSkillInventory : MonoBehaviour
{
    [SerializeField] protected ReuseScrollview<SkillData> reuseScrollview;


    protected UIManager UIManager => UIManager.Instance;
    protected Func<List<SkillData>> GetSkillInventorySource;

    protected Dictionary<SkillData, SkillSlot> skillToSlotMap { get; private set; } = new();

    public virtual void Initialize(Func<List<SkillData>> inventoryGetter, Action<SkillSlot> onClickHandler)
    {
        GetSkillInventorySource = inventoryGetter;

        reuseScrollview.SetData(GetSkillInventorySource());
        skillToSlotMap.Clear();
        List<SkillData> dataList = GetSkillInventorySource();

        int count = Mathf.Min(reuseScrollview.ItemList.Count, dataList.Count);
        for (int i = 0; i < count; i++)
        {
            if (reuseScrollview.ItemList[i].TryGetComponent<SkillSlot>(out var slot))
            {
                slot.SetOnClickCallback(onClickHandler);
                skillToSlotMap.Add(dataList[i], slot);
            }
        }
    }

    public ScrollData<SkillData> GetDataByItem(SkillData item)
    {
        return reuseScrollview.GetDataFromItem(item);
    }

    public void RefreshAtSlotUI(SkillData item)
    {
        int index = reuseScrollview.GetDataIndexFromItem(item);
        reuseScrollview.RefreshSlotAt(index);
    }
}