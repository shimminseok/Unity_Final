using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerUnitTable", menuName = "Table/PlayerUnitTable", order = 0)]
public class PlayerUnitTable : BaseTable<int, PlayerUnitSO>
{
    protected override string[] DataPath => new[] { "Assets/10. Tables/Unit/Hero" };

    public Dictionary<JobType, List<PlayerUnitSO>> PlayerUnitByJob { get; private set; } = new Dictionary<JobType, List<PlayerUnitSO>>();

    public override void CreateTable()
    {
        Type = GetType();
        foreach (PlayerUnitSO data in dataList)
        {
            DataDic[data.ID] = data;

            foreach (JobType job in Enum.GetValues(typeof(JobType)))
            {
                AddToPlayerUnitByJob(job, data);
            }
        }
    }

    private void AddToPlayerUnitByJob(JobType jobType, PlayerUnitSO playerUnit)
    {
        if (!PlayerUnitByJob.TryGetValue(jobType, out var jobEquipList))
        {
            jobEquipList = new List<PlayerUnitSO>();
            PlayerUnitByJob[jobType] = jobEquipList;
        }

        jobEquipList.Add(playerUnit);
    }

    public List<PlayerUnitSO> GetPlayerUnitsByJob(JobType jobType)
    {
        return PlayerUnitByJob.TryGetValue(jobType, out var playerUnitList) ? playerUnitList : new List<PlayerUnitSO>();
    }
}