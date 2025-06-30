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
        Owner = unit;
        Owner.SkillController.Initialize(this);
        foreach (ActiveSkillSO activeSkillSo in selectedSkill)
        {
            SkillData skillData = new SkillData
            (
                activeSkillSo.skillName,
                activeSkillSo.skillDescription,
                activeSkillSo.skillType,
                activeSkillSo.selectCamp,
                activeSkillSo.selectType,
                activeSkillSo.mainSkillEffect,
                activeSkillSo.subSkillEffect,
                activeSkillSo.jobType,
                activeSkillSo.reuseMaxCount,
                activeSkillSo.coolTime,
                activeSkillSo.skillIcon,
                activeSkillSo.SkillVFX,
                activeSkillSo.skillAnimation
            );
            skillData.mainEffect.owner = Owner;
            skillData.subEffect.owner = Owner;
            skillData.mainEffect.Init();
            skillData.subEffect.Init();
            Owner.SkillController.skills.Add(skillData);
        }
    }
}