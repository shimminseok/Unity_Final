using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewRangeAttack", menuName = "ScriptableObjects/AttackType/Range", order = 0)]
public class RangeAttackSO : RangeActionSo
{
    public override AttackDistanceType DistanceType => AttackDistanceType.Range;
    public override CombatActionSo     ActionSo     => this;

    public override void Execute(Unit attacker)
    {
        if (attacker.Target != null)
        {
            ProjectileComponent = ObjectPoolManager.Instance.GetObject(projectilePoolId).GetComponent<SkillProjectile>();
            ProjectileComponent.Initialize(attacker, attacker.GetCenter(), attacker.Target.Collider.bounds.center);

            ProjectileComponent.trigger.OnTriggerTarget += ResetProjectile;
        }
    }

    private void ResetProjectile()
    {
        ProjectileComponent = null;
    }
}