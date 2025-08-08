using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "PassiveSkillTable", menuName = "Table/PassiveSkillTable", order = 0)]
public class PassiveSkillTable : BaseTable<int, PassiveSO>
{
    protected override string[] DataPath => new[] { "Assets/10. Tables/Skill/Passive" };

    public Dictionary<JobType, List<PassiveSO>> PassiveSkillByJob { get; private set; } = new Dictionary<JobType, List<PassiveSO>>();

    public override void CreateTable()
    {
        Type = GetType();
        foreach (PassiveSO passiveSo in dataList)
        {
            if (!DataDic.TryAdd(passiveSo.ID, passiveSo))
            {
                Debug.LogWarning($"중복된 Passive ID 감지: {passiveSo.ID} - {passiveSo.name}");
            }

            if (!PassiveSkillByJob.TryGetValue(passiveSo.jobType, out List<PassiveSO> passiveSkills))
            {
                passiveSkills = new List<PassiveSO>();
                PassiveSkillByJob[passiveSo.jobType] = passiveSkills;
            }

            passiveSkills.Add(passiveSo);
        }
    }

    public List<PassiveSO> GetPassiveSkillsByJob(JobType jobType)
    {
        return PassiveSkillByJob.TryGetValue(jobType, out List<PassiveSO> passiveSkills) ? passiveSkills : new List<PassiveSO>();
    }
}