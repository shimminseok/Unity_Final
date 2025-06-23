using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageTable", menuName = "Table/StageTable", order = 0)]
public class StatgeTable : BaseTable<int, StageSO>
{
    protected override string[] DataPath => new[] { "Assets/10. Tables/Stage" };

    public override void CreateTable()
    {
        foreach (StageSO data in DataDic.Values)
        {
            DataDic[data.ID] = data;
        }
    }
}