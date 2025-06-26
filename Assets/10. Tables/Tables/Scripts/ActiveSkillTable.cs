using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActiveSkillTable", menuName = "Table/ActiveSkillTable", order = 0)]
public class ActiveSkillTable : BaseTable<int, SkillData>
{
    protected override string[] DataPath => new[] { "Assets/10. Tables/Skill/Active" };

    public Dictionary<JobType, List<SkillData>> ActiveSkillByJob { get; private set; } = new Dictionary<JobType, List<SkillData>>();

    public override void CreateTable()
    {
        Type = GetType();
        foreach (SkillData skillData in dataList)
        {
            if (!DataDic.TryAdd(skillData.ID, skillData))
            {
                Debug.LogWarning($"중복된 Passive ID 감지: {skillData.ID} - {skillData.name}");
            }

            if (!ActiveSkillByJob.TryGetValue(skillData.jobType, out List<SkillData> activeSkillData))
            {
                activeSkillData = new List<SkillData>();
                ActiveSkillByJob[skillData.jobType] = activeSkillData;
            }

            activeSkillData.Add(skillData);
        }
    }

    public List<SkillData> GetActiveSkillsByJob(JobType jobType)
    {
        return ActiveSkillByJob.TryGetValue(jobType, out List<SkillData> passiveSkills) ? passiveSkills : new List<SkillData>();
    }
}