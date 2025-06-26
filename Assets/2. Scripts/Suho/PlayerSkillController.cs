using System.Collections.Generic;
using UnityEngine;
/*
 * 플레이어 스킬 컨트롤러
 *
 * 1. ChangeSkill로 사용할 스킬을 정한다.
 *
 * 2. SelectTargets로 MainTareget과 SubTargets을 정한다
 *
 * 3. UseSkill()을 사용한다.
 *
 * 주의!!
 *  - SelectTargets를 먼저 사용할 경우 에러 발생.
 *
 * 현재 EndTurn을 스킬을 사용 후 바로 적용시켜줄 지 혹은 외부에서 호출해줄지 결정 중. [2025/06/24]
 * 스킬 사용 시 효과를 발생시키는 메서드 처리에 대한 고민 중 [2025/06/24]
 * 
 */
[RequireComponent(typeof(SkillManager))]
public class PlayerSkillController : BaseSkillController
{

    public override void SelectTargets(Unit target)
    {
        this.mainTarget = target;
        TargetSelect targetSelect = new TargetSelect();
        subTargets = targetSelect.FindTargets(target,currentSkill.selectedType,currentSkill.selectedCamp);
    }

    public void ChangeSkill(int index)
    {
        currentSkill = skills[index];
    }

    [ContextMenu("스킬 사용!")]
    public override void UseSkill()
    {
        currentSkill.cost = 0;
        currentSkill.reuseNumber++;
        if (mainTarget != null)
        {
            currentSkill.mainEffect.AffectTargetWithSkill(mainTarget);
        }
        // 메인타겟에 메인효과 적용
        

        if (subTargets != null)
        {
            foreach (Unit subTarget in subTargets)
            {
                currentSkill.subEffect.AffectTargetWithSkill(subTarget);
            }
        }
        // 서브타겟에 서브효과 적용
        
        currentSkill = null;
        
        EndTurn();
    }
    
    

    public override void EndTurn()
    {
        currentSkill = null;
        this.mainTarget = null;
        subTargets = null;
        foreach (Skill skill in skills)
        {
            skill.RegenerateCost(generateCost);
        }
    }
}
