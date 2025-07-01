using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SkillManager : MonoBehaviour
{
    public List<ActiveSkillSO> selectedSkill;
    public Unit Owner { get; private set; }


    public void InitializeSkillManager(Unit unit)
    {
        //테스트 스킬로 들어옴
        Owner = unit;
        Owner.SkillController.Initialize(this);
        foreach (ActiveSkillSO activeSkillSo in selectedSkill)
        {
            if (activeSkillSo == null)
            {
                Owner.SkillController.skills.Add(null);
                continue;
            }

            SkillData skillData = new SkillData
            (
                activeSkillSo.skillName,
                activeSkillSo.skillDescription,
                activeSkillSo.skillType,
                activeSkillSo.selectCamp,
                activeSkillSo.skillEffect,
                activeSkillSo.jobType,
                activeSkillSo.reuseMaxCount,
                activeSkillSo.coolTime,
                activeSkillSo.skillIcon,
                activeSkillSo.SkillVFX,
                activeSkillSo.skillAnimation
            );
            skillData.skillEffect.owner = Owner;
            skillData.skillEffect.Init();
            Owner.SkillController.skills.Add(skillData);
        }
    }
}