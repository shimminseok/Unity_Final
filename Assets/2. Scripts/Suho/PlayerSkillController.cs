using System;
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
[System.Serializable]
[RequireComponent(typeof(SkillManager))]
public class PlayerSkillController : BaseSkillController
{

    public Animator animator;
    
    public AnimationClip skillAttackAnim;
    private SkillAnimationListener skillAnimationListener;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        skillAnimationListener = GetComponent<SkillAnimationListener>();
    }

    public override void SelectTargets(Unit target)
    {
        this.mainTarget = target;
        TargetSelect targetSelect = new TargetSelect();
        subTargets = targetSelect.FindTargets(target, CurrentSkillData.selectedType, CurrentSkillData.selectedCamp);
    }

    public void ChangeSkill(int index)
    {
        CurrentSkillData = skills[index];
        // AnimatorOverrideController 교체
        AnimatorOverrideController overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        overrideController["ATK2"] = CurrentSkillData.skillAnimation;
        animator.runtimeAnimatorController = overrideController;
        skillAnimationListener.skillData = CurrentSkillData;
    }

    public override void UseSkill()
    {
        if (!CurrentSkillData.CheckCanUseSkill())
        {
            Debug.LogWarning("사용 불가능한 스킬 사용시도");
            return;
        }
        CurrentSkillData.coolDown = CurrentSkillData.coolTime;
        CurrentSkillData.reuseCount--;
        SelectTargets(mainTarget);
        //CurrentSkillData.skillType.UseSkill(this);
  
    }


    public override void EndTurn()
    {
        CurrentSkillData = null;
        this.mainTarget = null;
        subTargets = null;
        foreach (SkillData skill in skills)
        {
            skill.RegenerateCoolDown(generateCost);
        }
    }
}