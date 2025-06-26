using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActiveSkillTable", menuName = "Table/ActiveSkillTable", order = 0)]
public class ActiveSkillTable : BaseTable<int, SkillData>
{
    protected override string[] DataPath => new[] { "Assets/10. Tables/Skill/Active" };

    public override void CreateTable()
    {
        Type = GetType();
        foreach (SkillData activeSkillData in dataList)
        {
            DataDic[activeSkillData.ID] = activeSkillData;
        }
    }
}