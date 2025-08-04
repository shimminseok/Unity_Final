using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewRangeAttack", menuName = "ScriptableObjects/AttackType/Range", order = 0)]
public class RangeAttackSO : RangeActionSo
{
    public override AttackDistanceType DistanceType => AttackDistanceType.Range;
    public override CombatActionSo     ActionSo     => this;
    public override bool               IsProjectile => !string.IsNullOrEmpty(projectilePoolID);
    
    public override void Execute(IAttackable attacker, IDamageable target)
    {
        if (target != null)
        {
            GameObject projectile = ObjectPoolManager.Instance.GetObject(projectilePoolID);
            PlaySFX(attacker);
            if (projectile == null)
            {
                projectile = Instantiate(projectilePrefab);
            }
            ProjectileComponent = projectile.GetComponent<PoolableProjectile>();
            ProjectileComponent.Initialize(attacker, attacker.Collider.bounds.center, target.Collider.bounds.center,target);

            ProjectileComponent.trigger.OnTriggerTarget += ResetProjectile;
        }
        
    }
    
    public override void PlaySFX(IAttackable attacker)
    {
        AudioManager.Instance.PlaySFX(AttackSound.ToString());
        AudioManager.Instance.PlaySFX(HitSound.ToString());
    }

    private void ResetProjectile()
    {
        ProjectileComponent = null;
    }
}