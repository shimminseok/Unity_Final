using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class TargetSelect
{   
    private Unit mainTargetUnit;
    public TargetSelect(Unit mainTarget)
    {
        mainTargetUnit = mainTarget;
    }
    public List<Unit> FindTargets(SelectTargetType type, SelectCampType camp)
    {
        
        switch (type)
        {
            case SelectTargetType.Single:
                List<Unit> singleTarget = new List<Unit>();
                singleTarget.Add(mainTargetUnit);
                return singleTarget;

            case SelectTargetType.All:
                List<Unit> combinedUnits = camp switch
                {
                    SelectCampType.Enemy => BattleManager.Instance.EnemyUnits,
                    SelectCampType.Player => BattleManager.Instance.PartyUnits,
                    SelectCampType.BothSide => BattleManager.Instance.PartyUnits
                        .Concat(BattleManager.Instance.EnemyUnits).ToList(),
                    _ => null
                };
                if (combinedUnits == null) return null;
                List<Unit> filteredUnits = combinedUnits.Where(u => u != mainTargetUnit).ToList();
                return filteredUnits;
            
            case SelectTargetType.RandomOne:
                combinedUnits = camp switch
                {
                    SelectCampType.Enemy => BattleManager.Instance.EnemyUnits,
                    SelectCampType.Player => BattleManager.Instance.PartyUnits,
                    SelectCampType.BothSide => BattleManager.Instance.PartyUnits
                        .Concat(BattleManager.Instance.EnemyUnits).ToList(),
                    _ => null
                };

                if (combinedUnits == null) return null;

                // mainTarget 제거 후 랜덤 선택
                filteredUnits = combinedUnits.Where(u => u != mainTargetUnit).ToList();
                if (filteredUnits.Count == 0) return new List<Unit>(); // 선택 가능한 유닛이 없을 경우

                Unit randomTarget = filteredUnits[Random.Range(0, filteredUnits.Count)];
                return new List<Unit> { randomTarget };

            default:
                return null;
        }
    }
}
