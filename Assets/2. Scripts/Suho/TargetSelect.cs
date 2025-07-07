using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

/*
 * TargetSelect => 메인타겟을 기준으로 SelectTargetType에 따라 실제로 목표로 정할 타겟들을 리턴해주는 클래스
 * attacker => 스킬을 사용하는 유닛도 타겟으로 지정가능해야 하므로 추가
 */
public class TargetSelect
{   
    private Unit mainTargetUnit;
    private Unit attacker;
    private int column = 3;
    public TargetSelect(Unit mainTarget, Unit attacker)
    {
        mainTargetUnit = mainTarget;
        this.attacker = attacker;
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

            case SelectTargetType.AllExceptMainTarget:
                if (filteredUnits.Count == 0) return targets; // 선택 가능한 유닛이 없을 경우
                return filteredUnits;
            
            case SelectTargetType.RandomOneExceptMainTarget:
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
                if(IsValidSector(tempTargetIndex,column,combinedUnits.Count) && !combinedUnits[tempTargetIndex].IsDead)
                    targets.Add(combinedUnits[tempTargetIndex]);
                if(IsValidSector(secondTempTargetIndex,column,combinedUnits.Count)&& !combinedUnits[secondTempTargetIndex].IsDead) 
                    targets.Add(combinedUnits[secondTempTargetIndex]);
                return targets;
            
            case SelectTargetType.onSelf:
                targets.Add(attacker);
                return targets;
            
            case SelectTargetType.All:
                filteredUnits.Add(mainTargetUnit);
                return filteredUnits;
            
            default:
                return null;
        }
    }
}
