using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerUnitTable", menuName = "Table/PlayerUnitTable", order = 0)]
public class PlayerUnitTable : BaseTable<int, PlayerUnitSO>
{
    // Start is called before the first frame update
    protected override string[] DataPath => new[] { "Assets/10. Tables/Unit/Hero" };


    public override void CreateTable()
    {
        Type = GetType();
        foreach (PlayerUnitSO data in dataList)
        {
            DataDic[data.ID] = data;
        }
    }
}