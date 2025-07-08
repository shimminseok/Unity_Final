using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewRangeAttack", menuName = "ScriptableObjects/AttackType/Range", order = 0)]
public class RangeAttackSO : RangeActionSo
{
    public override AttackDistanceType DistanceType => AttackDistanceType.Range;
    public override CombatActionSo     ActionSo     => this;

    public override void Execute(IAttackable attacker, IDamageable target)
    {
        var skillController = attacker.SkillController;
        if (target != null)
        {
            ProjectileComponent = ObjectPoolManager.Instance.GetObject(projectilePoolId).GetComponent<SkillProjectile>();
            ProjectileComponent.Initialize(attacker, skillController.SkillManager.Owner.GetCenter(), target.Collider.bounds.center);

            ProjectileComponent.trigger.OnTriggerTarget += ResetProjectile;
        }
    }

    private void ResetProjectile()
    {
        ProjectileComponent = null;
    }
}