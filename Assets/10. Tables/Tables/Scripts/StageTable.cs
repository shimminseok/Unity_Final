using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageTable", menuName = "Table/StageTable", order = 0)]
public class StageTable : BaseTable<int, StageSO>
{
    protected override string[] DataPath => new[] { "Assets/10. Tables/Stage" };

    public override void CreateTable()
    {
        Type = GetType();
        foreach (StageSO data in dataList)
        {
            DataDic[data.ID] = data;
        }
    }
}