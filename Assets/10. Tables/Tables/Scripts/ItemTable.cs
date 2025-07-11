using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemTable", menuName = "Table/ItemTable", order = 0)]
public class ItemTable : BaseTable<int, ItemSO>
{
    protected override string[] DataPath => new[] { "Assets/10. Tables/Item" };

    public Dictionary<JobType, List<EquipmentItemSO>> EquipmentByJob { get; private set; } = new Dictionary<JobType, List<EquipmentItemSO>>();

    public Dictionary<(EquipmentType, Tier), List<EquipmentItemSO>> EquipmentByTypeAndTier { get; private set; } = new Dictionary<(EquipmentType, Tier), List<EquipmentItemSO>>();

    public override void CreateTable()
    {
        Type = GetType();
        foreach (ItemSO item in dataList)
        {
            if (!DataDic.TryAdd(item.ID, item))
            {
                Debug.LogWarning($"중복된 아이템 ID 감지: {item.ID} - {item.name}");
            }

            DataDic[item.ID] = item;

            if (item is not EquipmentItemSO equipmentItem)
                continue;

            //장비 종류, 티어별로 아이템을 모아둔 딕셔너리
            AddToEquipmentByTypeAndTier(equipmentItem);

            if (equipmentItem.IsEquipableByAllJobs)
            {
                foreach (JobType job in Enum.GetValues(typeof(JobType)))
                {
                    AddToEquipmentByJob(job, equipmentItem);
                }
            }
            else
            {
                AddToEquipmentByJob(equipmentItem.JobType, equipmentItem);
            }
        }
    }

    private void AddToEquipmentByJob(JobType jobType, EquipmentItemSO equipmentItem)
    {
        if (!EquipmentByJob.TryGetValue(jobType, out var jobEquipList))
        {
            jobEquipList = new List<EquipmentItemSO>();
            EquipmentByJob[jobType] = jobEquipList;
        }

        jobEquipList.Add(equipmentItem);
    }

    private void AddToEquipmentByTypeAndTier(EquipmentItemSO equipmentItem)
    {
        var key = (equipmentItem.EquipmentType, equipmentItem.Tier);
        if (!EquipmentByTypeAndTier.TryGetValue(key, out var list))
        {
            list = new List<EquipmentItemSO>();
            EquipmentByTypeAndTier[key] = list;
        }

        list.Add(equipmentItem);
    }

    public List<EquipmentItemSO> GetEquipmentsByJob(JobType jobType)
    {
        return EquipmentByJob.TryGetValue(jobType, out var equipmentList) ? equipmentList : new List<EquipmentItemSO>();
    }

    public List<EquipmentItemSO> GetEquipmentsByTypeAndTier(EquipmentType equipmentType, Tier tier)
    {
        return EquipmentByTypeAndTier.TryGetValue((equipmentType, tier), out var equipmentList) ? equipmentList : new List<EquipmentItemSO>();
    }
}