using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActiveSkillTable", menuName = "Table/ActiveSkillTable", order = 0)]
public class ActiveSkillTable : BaseTable<int, ActiveSkillSO>
{
    protected override string[] DataPath => new[] { "Assets/10. Tables/Skill/Active" };

    public Dictionary<JobType, List<ActiveSkillSO>> ActiveSkillByJob { get; private set; } = new Dictionary<JobType, List<ActiveSkillSO>>();

    public override void CreateTable()
    {
        Type = GetType();
        foreach (ActiveSkillSO skillData in dataList)
        {
            if (!DataDic.TryAdd(skillData.ID, skillData))
            {
                Debug.LogWarning($"중복된 Passive ID 감지: {skillData.ID} - {skillData.name}");
            }

            if (!ActiveSkillByJob.TryGetValue(skillData.jobType, out List<ActiveSkillSO> activeSkillData))
            {
                activeSkillData = new List<ActiveSkillSO>();
                ActiveSkillByJob[skillData.jobType] = activeSkillData;
            }

            activeSkillData.Add(skillData);
        }
    }

    public List<ActiveSkillSO> GetActiveSkillsByJob(JobType jobType)
    {
        return ActiveSkillByJob.TryGetValue(jobType, out List<ActiveSkillSO> passiveSkills) ? passiveSkills : new List<ActiveSkillSO>();
    }
}