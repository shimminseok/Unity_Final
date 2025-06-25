using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class TargetSelect
{
    public List<IDamageable> FindTargets(IDamageable mainTarget, SelectTargetType type,SelectCampType camp)
    {
        switch (type)
        { 
            case SelectTargetType.Single:
                return null;
            
            case SelectTargetType.All:
                if (camp == SelectCampType.Enemy)
                {
                    List<IDamageable> targets = BattleManager.Instance.EnemyUnits.ConvertAll(new Converter<Unit, IDamageable>(UnitToIDamageable));
                    return targets;
                }
                else if (camp == SelectCampType.Player)
                {
                    List<IDamageable> targets = BattleManager.Instance.PartyUnits.ConvertAll(new Converter<Unit, IDamageable>(UnitToIDamageable));
                    return targets;
                }
                else if (camp == SelectCampType.BothSide)
                {
                    List<IDamageable> playerUnit =
                        BattleManager.Instance.PartyUnits.ConvertAll(
                            new Converter<Unit, IDamageable>(UnitToIDamageable));
                    List<IDamageable> enemyUnit =
                        BattleManager.Instance.EnemyUnits.ConvertAll(
                            new Converter<Unit, IDamageable>(UnitToIDamageable));
                    List<IDamageable> total = playerUnit.Concat(enemyUnit).ToList();
                    return total;
                }
                else return null;
            
            case SelectTargetType.SinglePlusRandomOne:
                if (camp == SelectCampType.Enemy)
                {
                    List<IDamageable> targets = BattleManager.Instance.EnemyUnits.ConvertAll(new Converter<Unit, IDamageable>(UnitToIDamageable));
                    List<IDamageable> target = new List<IDamageable> { targets[Random.Range(0, targets.Count)] };
                    return target;
                }
                else if (camp == SelectCampType.Player)
                {
                    List<IDamageable> targets = BattleManager.Instance.PartyUnits.ConvertAll(new Converter<Unit, IDamageable>(UnitToIDamageable));
                    List<IDamageable> target = new List<IDamageable> { targets[Random.Range(0, targets.Count)] };
                    return target;
                }
                else if (camp == SelectCampType.BothSide)
                {
                    List<IDamageable> playerUnit =
                        BattleManager.Instance.PartyUnits.ConvertAll(
                            new Converter<Unit, IDamageable>(UnitToIDamageable));
                    List<IDamageable> enemyUnit =
                        BattleManager.Instance.EnemyUnits.ConvertAll(
                            new Converter<Unit, IDamageable>(UnitToIDamageable));
                    List<IDamageable> total = playerUnit.Concat(enemyUnit).ToList();
                    List<IDamageable> target = new List<IDamageable> { total[Random.Range(0, total.Count)] };
                    return target;
                }
                else return null;

            default:
                return null;
        }

    }

    public static IDamageable UnitToIDamageable(Unit unit)
    {
        IDamageable damageable = unit;
        return damageable;
    } 
    
}

