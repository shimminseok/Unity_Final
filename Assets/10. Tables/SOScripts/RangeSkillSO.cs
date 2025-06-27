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
            projectile.Initialize(skillController.CurrentSkillData.mainEffect, skillController.transform.position, skillController.mainTarget.transform.position, skillController.mainTarget);
        }


        if (skillController.subTargets != null)
        {
            foreach (Unit subTarget in skillController.subTargets)
            {
                SkillProjectile projectile = ObjectPoolManager.Instance.GetObject(subProjectilePoolID).GetComponent<SkillProjectile>();
                projectile.Initialize(skillController.CurrentSkillData.subEffect, skillController.transform.position, subTarget.transform.position, subTarget);
            }
        }
    }

    public override AttackDistanceType DistanceType => AttackDistanceType.Range;
}