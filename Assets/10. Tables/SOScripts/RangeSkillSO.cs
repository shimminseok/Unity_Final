using UnityEngine;

[CreateAssetMenu(fileName = "NewRangeSkillSO", menuName = "ScriptableObjects/SKillType/Range", order = 0)]
public class RangeSkillSO : SkillTypeSO
{
    public string mainProjectilePoolID;
    public string subProjectilePoolID;

    public override void UseSkill(BaseSkillController controller)
    {
        this.skillController = controller;

        if (skillController.mainTarget != null)
        {
            SkillProjectile projectile = ObjectPoolManager.Instance.GetObject(mainProjectilePoolID).GetComponent<SkillProjectile>();
            projectile.Initialize(skillController.CurrentSkillData.mainEffect, skillController.SkillManager.Owner.GetCenter(), skillController.mainTarget.GetCenter(), skillController.mainTarget);
        }


        if (skillController.subTargets != null)
        {
            foreach (Unit subTarget in skillController.subTargets)
            {
                SkillProjectile projectile = ObjectPoolManager.Instance.GetObject(subProjectilePoolID).GetComponent<SkillProjectile>();
                projectile.Initialize(skillController.CurrentSkillData.subEffect, skillController.SkillManager.Owner.GetCenter(), skillController.mainTarget.GetCenter(), subTarget);
            }
        }
    }

    public override AttackDistanceType DistanceType => AttackDistanceType.Range;
}