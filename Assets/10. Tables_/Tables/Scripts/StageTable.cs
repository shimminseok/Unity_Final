using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageTable", menuName = "Table/StageTable", order = 0)]
public class StageTable : BaseTable<int, StageSO>
{
    protected override string[] DataPath => new[] { "Assets/10. Tables/Stage" };


    private readonly Dictionary<int, List<StageSO>> stagesByChapterMap = new();
    public override void CreateTable()
    {
        Type = GetType();
        foreach (StageSO data in dataList)
        {
            DataDic[data.ID] = data;
            int chapter = data.ID / 1000000;
            if (!stagesByChapterMap.TryGetValue(chapter, out List<StageSO> stages))
            {
                stages = new List<StageSO>();
                stagesByChapterMap[chapter] = stages;
            }

            stages.Add(data);
        }
    }


    public List<StageSO> GetStagesByChapter(int chapter)
    {
        return stagesByChapterMap.TryGetValue(chapter, out List<StageSO> stages) ? stages : new List<StageSO>();
    }
}