using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class TargetSelect
{   
    private Unit mainTargetUnit;
    private int column = 3;
    public TargetSelect(Unit mainTarget)
    {
        mainTargetUnit = mainTarget;
    }

    public bool IsValidSector(int tempTargetindex, int column, int length)
    {
        if (tempTargetindex / column <= 0) return false;
        if (tempTargetindex >= length) return false;
        return true;
    }
    public List<Unit> FindTargets(SelectTargetType type, SelectCampType camp)
    {
        List<Unit> targets = new List<Unit>();
        if(mainTargetUnit == null) return null;
        List<Unit> combinedUnits = camp switch
        {
            SelectCampType.Enemy => BattleManager.Instance.EnemyUnits,
            SelectCampType.Player => BattleManager.Instance.PartyUnits,
            SelectCampType.BothSide => BattleManager.Instance.PartyUnits
                .Concat(BattleManager.Instance.EnemyUnits).ToList(),
            _ => null
        };
         // mainTarget과 죽은 유닛 제거
        List<Unit> filteredUnits = combinedUnits.Where(u => u != mainTargetUnit && !u.IsDead).ToList();
        if (combinedUnits == null) return targets;
        switch (type)
        {
            case SelectTargetType.MainTarget:
                targets.Add(mainTargetUnit);
                return targets;

            case SelectTargetType.All:
                if (filteredUnits.Count == 0) return targets; // 선택 가능한 유닛이 없을 경우
                return filteredUnits;
            
            case SelectTargetType.RandomOne:
                if (filteredUnits.Count == 0) return targets; // 선택 가능한 유닛이 없을 경우
                Unit randomTarget = filteredUnits[Random.Range(0, filteredUnits.Count)];
                targets.Add(randomTarget);
                return targets;
            
            case SelectTargetType.Sector:
                combinedUnits = camp switch
                {
                    SelectCampType.Enemy => BattleManager.Instance.EnemyUnits,
                    SelectCampType.Player => BattleManager.Instance.PartyUnits,
                    _ => null
                };
                int mainTargetIndex = combinedUnits.IndexOf(mainTargetUnit);
                int tempTargetIndex = mainTargetIndex + column-1;
                int secondTempTargetIndex = mainTargetIndex + column;
                if(IsValidSector(tempTargetIndex,column,combinedUnits.Count) && !combinedUnits[tempTargetIndex].IsDead) targets.Add(combinedUnits[tempTargetIndex]);
                if(IsValidSector(secondTempTargetIndex,column,combinedUnits.Count)&& !combinedUnits[secondTempTargetIndex].IsDead) targets.Add(combinedUnits[secondTempTargetIndex]);
                return targets;
            
            default:
                return null;
        }
    }
}
