using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterTable", menuName = "Table/MonsterTable", order = 0)]
public class MonsterTable : BaseTable<int, EnemyUnitSO>
{
    protected override string[] DataPath => new[] { "Assets/10. Tables/Unit/Monster" };

    public override void CreateTable()
    {
        Type = GetType();
        foreach (EnemyUnitSO data in dataList)
        {
            DataDic[data.ID] = data;
        }
    }
}