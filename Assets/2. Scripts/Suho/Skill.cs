using System.Collections.Generic;
using UnityEngine;

public class Skill
{
    public Skill(SkillTypeSO type,SelectCampType camp, SelectTargetType selectType, StatBaseSkillEffect mainEffect, StatBaseSkillEffect subEffect, JobType jobType, int reuseMaxNumber, int maxCost, Sprite skillIcon, ParticleSystem.Particle skillVFX, AnimationClip skillAnimation)
    {
        this.skillType = type;
        this.selectedCamp = camp;
        this.selectedType = selectType;
        this.mainEffect = mainEffect;
        this.subEffect = subEffect;
        this.jobType = jobType;
        this.reuseMaxNumber = reuseMaxNumber;
        this.maxCost = maxCost;
        this.cost = maxCost;
        this.skillIcon = skillIcon;
        SkillVFX = skillVFX;
        this.skillAnimation = skillAnimation;
    }

    //파워 슬래쉬
    public SkillTypeSO skillType; // 원거리인가 근거리인가
    public SelectCampType selectedCamp;

    public SelectTargetType selectedType = SelectTargetType.Single;

    public StatBaseSkillEffect mainEffect;
    public StatBaseSkillEffect subEffect;
    public JobType jobType;
    public int reuseNumber = 0;
    public int cost = 0;
    public int reuseMaxNumber;
    public int maxCost;
    public Sprite skillIcon;
    public ParticleSystem.Particle SkillVFX;
    public AnimationClip skillAnimation;

    public void RegenerateCost(int value)
    {
        cost = Mathf.Min(maxCost, cost + value);
    }

    public bool CheckCanUseSkill()
    {
        if (cost < maxCost)
        {
            return false;
        }

        if (reuseNumber > reuseMaxNumber)
        {
            return false;
        }

        return true;
    }
}