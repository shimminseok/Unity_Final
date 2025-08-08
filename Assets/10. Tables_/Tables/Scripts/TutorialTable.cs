using UnityEngine;

[CreateAssetMenu(fileName = "TutorialTable", menuName = "Table/TutorialTable", order = 0)]
public class TutorialTable : BaseTable<int, TutorialStepSO>
{
    protected override string[] DataPath => new[] { "Assets/10. Tables/TutorialStep" };

    public override void CreateTable()
    {
        Type = GetType();
        foreach (TutorialStepSO step in dataList)
        {
            DataDic[step.ID] = step;
        }
    }
}
