using UnityEngine;

[CreateAssetMenu(fileName = "NewRangeAttack", menuName = "ScriptableObjects/AttackType/Range", order = 0)]
public class RangeAttackSO : AttackTypeSO
{
    public override AttackDistanceType DistanceType => AttackDistanceType.Range;

    public override void Attack(Unit attacker)
    {
        attacker.Target.TakeDamage(attacker.StatManager.GetValue(StatType.AttackPow));
    }
}