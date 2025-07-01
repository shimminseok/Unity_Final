using System.Collections.Generic;
using UnityEngine;

public class SkillData
{
    public SkillData(string name, string des, CombatActionSo type, SelectCampType camp, SelectTargetType selectType, StatBaseSkillEffect mainEffect, StatBaseSkillEffect subEffect, JobType jobType, int reuseMaxCount, int maxCoolTime, Sprite skillIcon, ParticleSystem.Particle skillVFX, AnimationClip skillAnimation)
    {
        this.skillName = name;
        this.skillDescription = des;
        this.skillType = type;
        this.selectedCamp = camp;
        this.selectedType = selectType;
        this.mainEffect = mainEffect;
        this.subEffect = subEffect;
        this.jobType = jobType;
        this.reuseMaxCount = reuseMaxCount;
        this.reuseCount = reuseMaxCount;
        this.coolDown = 0;
        this.coolTime = maxCoolTime;
        this.skillIcon = skillIcon;
        SkillVFX = skillVFX;
        this.skillAnimation = skillAnimation;
    }

    public string skillName;
    public string skillDescription;
    public CombatActionSo skillType; // 원거리인가 근거리인가
    public SelectCampType selectedCamp;
    public SelectTargetType selectedType = SelectTargetType.Single;
    public StatBaseSkillEffect mainEffect;
    public StatBaseSkillEffect subEffect;
    public JobType jobType;
    public int reuseCount;
    public int coolDown = 0;
    public int reuseMaxCount;
    public int coolTime;
    public Sprite skillIcon;
    public ParticleSystem.Particle SkillVFX;
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