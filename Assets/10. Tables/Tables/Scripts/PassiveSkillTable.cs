using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PassiveSkillTable", menuName = "Table/PassiveSkillTable", order = 0)]
public class PassiveSkillTable : BaseTable<int, PassiveSO>
{
    protected override string[] DataPath => new[] { "Assets/10. Tables/Skill/Passive" };

    public override void CreateTable()
    {
        Type = GetType();
        foreach (PassiveSO passiveSo in dataList)
        {
            DataDic[passiveSo.PassiveID] = passiveSo;
        }
    }
}