using System.Buffers;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRangeSkillSO", menuName = "ScriptableObjects/SKillType/Range", order = 0)]
public class RangeSkillSO : RangeActionSo
{
    public string subProjectilePoolID;

    public override AttackDistanceType DistanceType => AttackDistanceType.Range;
    public override CombatActionSo     ActionSo     => this;

    public override void Execute(Unit attacker, IDamageable target)
    {
        var skillController = attacker.SkillController;

        TargetSelect targetSelect = new TargetSelect(target as Unit, attacker as Unit);

        foreach (var effect in skillController.CurrentSkillData.skillEffect.skillEffectDatas)
        {
            skillController.targets = targetSelect.FindTargets(effect.selectTarget, effect.selectCamp);
            string projectileID = effect.projectilePrefab.GetComponent<PoolableProjectile>().PoolID;
            foreach (Unit unit in skillController.targets)
            {
                if (unit == null) continue;
                GameObject projectile = ObjectPoolManager.Instance.GetObject(projectileID);
                if (projectile == null)
                {
                    projectile = Instantiate(effect.projectilePrefab);
                }
                ProjectileComponent = projectile.GetComponent<PoolableProjectile>();
                ProjectileComponent.Initialize(effect, skillController.SkillManager.Owner.GetCenter(), unit.GetCenter(), unit);

                if (ProjectileComponent != null)
                { 
                    ProjectileComponent.trigger.OnTriggerTarget += ResetProjectile;
                }
                
            }
        }


    }


    private void ResetProjectile()
    {
        ProjectileComponent = null;
    }
}