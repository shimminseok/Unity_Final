using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class TargetSelect
{
    public List<Unit> FindTargets(Unit mainTarget, SelectTargetType type,SelectCampType camp)
    {
        switch (type)
        { 
            case SelectTargetType.Single:
                return null;
            
            case SelectTargetType.All:
                if (camp == SelectCampType.Enemy)
                {
                    List<Unit> targets = BattleManager.Instance.EnemyUnits;
                    return targets;
                }
                else if (camp == SelectCampType.Player)
                {
                    List<Unit> targets = BattleManager.Instance.PartyUnits;
                    return targets;
                }
                else if (camp == SelectCampType.BothSide)
                {
                    List<Unit> playerUnit = BattleManager.Instance.PartyUnits;
                    List<Unit> enemyUnit =  BattleManager.Instance.EnemyUnits;
                   
                    List<Unit> total = playerUnit.Concat(enemyUnit).ToList();
                    return total;
                }
                else return null;
            
            case SelectTargetType.SinglePlusRandomOne:
                if (camp == SelectCampType.Enemy)
                {
                    List<Unit> targets = BattleManager.Instance.EnemyUnits;
                    List<Unit> target = new List<Unit> { targets[Random.Range(0, targets.Count)] };
                    return target;
                }
                else if (camp == SelectCampType.Player)
                {
                    List<Unit> targets = BattleManager.Instance.PartyUnits;
                    List<Unit> target = new List<Unit> { targets[Random.Range(0, targets.Count)] };
                    return target;
                }
                else if (camp == SelectCampType.BothSide)
                {
                    List<Unit> playerUnit = BattleManager.Instance.PartyUnits;
                    List<Unit> enemyUnit = BattleManager.Instance.EnemyUnits;
                    List<Unit> total = playerUnit.Concat(enemyUnit).ToList();
                    List<Unit> target = new List<Unit> { total[Random.Range(0, total.Count)] };
                    return target;
                }
                else return null;

            default:
                return null;
        }

    }


    
}

