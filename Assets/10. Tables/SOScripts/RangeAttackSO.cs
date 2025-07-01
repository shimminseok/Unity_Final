using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewRangeAttack", menuName = "ScriptableObjects/AttackType/Range", order = 0)]
public class RangeAttackSO : AttackTypeSO
{
    public string projectilePoolId;
    public override AttackDistanceType DistanceType => AttackDistanceType.Range;

    public override void Attack(Unit attacker)
    {
        if (attacker.Target != null)
        {
            GameObject      projectile      = ObjectPoolManager.Instance.GetObject(projectilePoolId);
            SkillProjectile skillProjectile = projectile.GetComponent<SkillProjectile>();
            skillProjectile.Initialize(attacker, attacker.GetCenter(), attacker.Target.Collider.bounds.center);
        }
    }
}