using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RewardTable", menuName = "Table/RewardTable", order = 0)]
public class RewardTable : BaseTable<string, RewardSo>
{
    protected override string[] DataPath => new[] { "Assets/10. Tables/Reward" };

    public override void CreateTable()
    {
        Type = GetType();
        foreach (RewardSo data in dataList)
        {
            DataDic[data.Id] = data;
        }
    }
}