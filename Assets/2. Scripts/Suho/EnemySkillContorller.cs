using System;
using UnityEngine;

public class EnemySkillContorller : BaseSkillController
{
    public WeightedSelector<SkillData> selector;

    public override void SelectTargets(Unit mainTarget)
    {
        this.mainTarget = mainTarget;
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
        CurrentSkillData.skillSo.skillType.Execute(SkillManager.Owner, mainTarget);

        EndTurn();
    }

    public void SelectSkill()
    {
        ChangeCurrentSkill(selector.Select());
    }

    public void InitSkillSelector()
    {
        selector = new WeightedSelector<SkillData>();
        EnemyUnitSO MonsterSo = SkillManager.Owner.UnitSo as EnemyUnitSO;
        if (MonsterSo == null) return;
        for (int i = 0; i < skills.Count; i++)
        {
            int index = i; // 캡처할 새로운 지역 변수
            var skill = skills[index];

            selector.Add(
                skill,
                () => MonsterSo.SkillDatas[index].individualProbability,
                () => skill.CheckCanUseSkill()
            );
        }
    }

    public override void EndTurn()
    {
        foreach (SkillData skill in skills)
        {
            if (skill == null || skill == CurrentSkillData)
                continue;
            skill.RegenerateCoolDown(generateCost);
        }

        CurrentSkillData = null;
        this.mainTarget = null;
        targets = null;
    }

    public override void ChangeCurrentSkill(int index)
    {
        CurrentSkillData = skills[index];
        if (CurrentSkillData == null)
            return;

        SkillManager.Owner.ChangeClip(Define.SkillClipName, CurrentSkillData.skillSo.skillAnimation);

        // skillAnimationListener.skillData = CurrentSkillData;
    }

    public void ChangeCurrentSkill(SkillData skill)
    {
        CurrentSkillData = skill;
        if (CurrentSkillData == null)
            return;
        SkillManager.Owner.ChangeClip(Define.SkillClipName, CurrentSkillData.skillSo.skillAnimation);
    }
}