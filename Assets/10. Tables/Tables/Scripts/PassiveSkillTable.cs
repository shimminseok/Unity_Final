using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public List<PassiveSO> GetPassiveSkillJobs(JobType jobType)
    {
        List<PassiveSO> getPassiveSkillJobs = DataDic.Where(s => s.Value.JobType == jobType).Select(s => s.Value).ToList();

        return getPassiveSkillJobs;
    }
}