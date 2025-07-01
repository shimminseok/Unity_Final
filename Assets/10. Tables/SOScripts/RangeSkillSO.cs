using UnityEngine;

[CreateAssetMenu(fileName = "NewRangeSkillSO", menuName = "ScriptableObjects/SKillType/Range", order = 0)]
public class RangeSkillSO : RangeActionSo
{
    public string subProjectilePoolID;

    public override AttackDistanceType DistanceType => AttackDistanceType.Range;
    public override CombatActionSo     ActionSo     => this;

    public override void Execute(Unit attacker)
    {
        var skillController = attacker.SkillController;

        if (skillController.mainTarget != null)
        {
            ProjectileComponent = ObjectPoolManager.Instance.GetObject(projectilePoolId).GetComponent<SkillProjectile>();
            ProjectileComponent.Initialize(skillController.CurrentSkillData.mainEffect, skillController.SkillManager.Owner.GetCenter(), skillController.mainTarget.GetCenter(), skillController.mainTarget);
        }


        if (skillController.subTargets != null)
        {
            foreach (Unit subTarget in skillController.subTargets)
            {
                SkillProjectile projectile = ObjectPoolManager.Instance.GetObject(subProjectilePoolID).GetComponent<SkillProjectile>();
                projectile.Initialize(skillController.CurrentSkillData.subEffect, skillController.SkillManager.Owner.GetCenter(), subTarget.GetCenter(), subTarget);
            }
        }

        ProjectileComponent.trigger.OnTriggerTarget += ResetProjectile;
    }


    private void ResetProjectile()
    {
        ProjectileComponent = null;
    }
}