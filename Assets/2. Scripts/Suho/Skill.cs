using System.Collections.Generic;
using UnityEngine;

public class Skill
{
    public Skill(SelectCampType camp, SelectTargetType selectType, StatBaseSkillEffect mainEffect, StatBaseSkillEffect subEffect, JobType jobType, int reuseMaxNumber, int maxCost, Sprite skillIcon, ParticleSystem.Particle skillVFX, AnimationClip skillAnimation)
    {   
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

        if ( reuseNumber > reuseMaxNumber)
        {
            return false;
        }
        
        return true;
    }
    
}
