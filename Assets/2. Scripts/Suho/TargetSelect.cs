using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class TargetSelect
{
    public List<Unit> FindTargets(Unit mainTarget, SelectTargetType type, SelectCampType camp)
    {
        switch (type)
        {
            case SelectTargetType.Single:
                return null;

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
                List<Unit> filteredUnits = combinedUnits.Where(u => u != mainTarget).ToList();
                return filteredUnits;
            
            case SelectTargetType.SinglePlusRandomOne:
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
                filteredUnits = combinedUnits.Where(u => u != mainTarget).ToList();
                if (filteredUnits.Count == 0) return new List<Unit>(); // 선택 가능한 유닛이 없을 경우

                Unit randomTarget = filteredUnits[Random.Range(0, filteredUnits.Count)];
                return new List<Unit> { randomTarget };

            default:
                return null;
        }
    }
}
