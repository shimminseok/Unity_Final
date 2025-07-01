using UnityEngine;

[CreateAssetMenu(fileName = "NewRangeSkillSO", menuName = "ScriptableObjects/SKillType/Range", order = 0)]
public class RangeSkillSO : SkillTypeSO
{

    public override void UseSkill(BaseSkillController controller)
    {
        //SkillTypeSO에 있는 UseSKill 진짜 UseSkill
        this.skillController = controller;
        TargetSelect targetSelect = new TargetSelect(skillController.mainTarget);

        foreach (var effect in controller.CurrentSkillData.skillEffect.skillEffectDatas)
        {
            controller.targets = targetSelect.FindTargets(effect.selectTarget,effect.selectCamp);
            foreach (Unit target in controller.targets)
            {
                if (target == null) continue;
                SkillProjectile projectile = ObjectPoolManager.Instance.GetObject(effect.projectileID).GetComponent<SkillProjectile>();
                projectile.Initialize(effect, skillController.SkillManager.Owner.GetCenter(), target.GetCenter(), target);
            }
        }
 
    }

    public override AttackDistanceType DistanceType => AttackDistanceType.Range;
}