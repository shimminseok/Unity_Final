using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewRangeAttack", menuName = "ScriptableObjects/AttackType/Range", order = 0)]
public class RangeAttackSO : RangeActionSo
{
    public override AttackDistanceType DistanceType => AttackDistanceType.Range;
    public override CombatActionSo     ActionSo     => this;

    public override void Execute(IAttackable attacker, IDamageable target)
    {
        if (target != null)
        {
            GameObject projectile = ObjectPoolManager.Instance.GetObject(projectilePoolID);
            if (projectile == null)
            {
                projectile = Instantiate(projectilePrefab);
            }
            ProjectileComponent = projectile.GetComponent<PoolableProjectile>();
            ProjectileComponent.Initialize(attacker, attacker.Collider.bounds.center, target.Collider.bounds.center);

            ProjectileComponent.trigger.OnTriggerTarget += ResetProjectile;
        }
        
    }

    private void ResetProjectile()
    {
        ProjectileComponent = null;
    }
}