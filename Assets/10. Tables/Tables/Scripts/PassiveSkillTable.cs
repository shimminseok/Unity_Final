using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "PassiveSkillTable", menuName = "Table/PassiveSkillTable", order = 0)]
public class PassiveSkillTable : BaseTable<int, PassiveSO>
{
    protected override string[] DataPath => new[] { "Assets/10. Tables/Skill/Passive" };

    public Dictionary<JobType, List<PassiveSO>> PassiveSkillJobs { get; private set; } = new Dictionary<JobType, List<PassiveSO>>();

    public override void CreateTable()
    {
        Type = GetType();
        foreach (PassiveSO passiveSo in dataList)
        {
            if (!DataDic.TryAdd(passiveSo.PassiveID, passiveSo))
            {
                Debug.LogWarning($"중복된 Passive ID 감지: {passiveSo.PassiveID} - {passiveSo.name}");
            }

            if (!PassiveSkillJobs.TryGetValue(passiveSo.JobType, out List<PassiveSO> passiveSkills))
            {
                passiveSkills = new List<PassiveSO>();
                PassiveSkillJobs[passiveSo.JobType] = passiveSkills;
            }

            passiveSkills.Add(passiveSo);
        }
    }

    public List<PassiveSO> GetPassiveSkillJobs(JobType jobType)
    {
        return PassiveSkillJobs.TryGetValue(jobType, out List<PassiveSO> passiveSkills) ? passiveSkills : new List<PassiveSO>();
    }
}