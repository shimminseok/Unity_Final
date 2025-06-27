using UnityEngine;

[CreateAssetMenu(fileName = "NewRangeAttack", menuName = "ScriptableObjects/AttackType/Range", order = 0)]
public class RangeAttackSO : AttackTypeSO
{
    public override AttackDistanceType DistanceType => AttackDistanceType.Range;

    public override void Attack(Unit attacker)
    {
        Debug.Log("나는 원거리 공격이에오");

        attacker.Target.TakeDamage(attacker.StatManager.GetValue(StatType.AttackPow));
    }
}