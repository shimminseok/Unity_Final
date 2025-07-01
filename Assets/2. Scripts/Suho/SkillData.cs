using System.Collections.Generic;
using UnityEngine;

public class SkillData
{
    public SkillData(string name, string des,SkillTypeSO type,SelectCampType camp, StatBaseSkillEffect skillEffect, JobType jobType, int reuseMaxCount, int maxCoolTime, Sprite skillIcon, AnimationClip skillAnimation)
    {
        this.skillName = name;
        this.skillDescription = des;
        this.skillType = type;
        this.selectedCamp = camp;
        this.skillEffect = skillEffect;
        this.jobType = jobType;
        this.reuseMaxCount = reuseMaxCount;
        this.reuseCount = reuseMaxCount;
        this.coolDown = 0;
        this.coolTime = maxCoolTime;
        this.skillIcon = skillIcon;
        this.skillAnimation = skillAnimation;
    }
    
    public string skillName;
    public string skillDescription;
    public SkillTypeSO skillType; // 원거리인가 근거리인가
    public SelectCampType selectedCamp;
    public StatBaseSkillEffect skillEffect;
    public JobType jobType;
    public int reuseCount;
    public int coolDown = 0;
    public int reuseMaxCount;
    public int coolTime;
    public Sprite skillIcon;
    public AnimationClip skillAnimation;

    public void RegenerateCoolDown(int value)
    {
        coolDown = Mathf.Max(0, coolDown - value);
    }

    public bool CheckCanUseSkill()
    {
        if (0 < coolDown)
        {
            return false;
        }

        if (reuseCount <= 0)
        {
            return false;
        }

        return true;
    }
}