using UnityEngine;

[CreateAssetMenu(fileName = "NewRangeSkillSO", menuName = "ScriptableObjects/SKillType/Range", order = 0)]

public class RangeSkillSO : SkillTypeSO
{
    public string mainProjectilePoolID;
    public string subProjectilePoolID;
    public override void UseSkill(BaseSkillController controller)
    {
        this.skillController = controller;
        SkillProjectile projectile = ObjectPoolManager.Instance.GetObject(mainProjectilePoolID).GetComponent<SkillProjectile>();
        projectile.Initialize(skillController.currentSkill, skillController.transform.position, skillController.mainTarget.transform.position);
        
        if (skillController.mainTarget != null)
        {
            skillController.currentSkill.mainEffect.AffectTargetWithSkill(skillController.mainTarget);
        }


        if (skillController.subTargets != null)
        {
            foreach (Unit subTarget in skillController.subTargets)
            {
                skillController.currentSkill.subEffect.AffectTargetWithSkill(subTarget);
            }
        }
        
    }
}
