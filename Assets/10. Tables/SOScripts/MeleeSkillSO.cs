using UnityEngine;

[CreateAssetMenu(fileName = "NewMeleeSkillSO", menuName = "ScriptableObjects/SKillType/Melee", order = 0)]

public class MeleeSkillSO : SkillTypeSO
{
    
    public override void UseSkill(BaseSkillController controller)
    {
        this.skillController = controller;
        if (controller.mainTarget != null)
        {
            controller.currentSkill.mainEffect.AffectTargetWithSkill(controller.mainTarget);
        }


        if (controller.subTargets != null)
        {
            foreach (Unit subTarget in controller.subTargets)
            {
                controller.currentSkill.mainEffect.AffectTargetWithSkill(subTarget);
            }
        }
        
    }
}
