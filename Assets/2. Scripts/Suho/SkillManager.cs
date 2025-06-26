using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
   public List<SkillData> selectedSkill;
   public BaseSkillController skillController;
   [HideInInspector]public Unit owner;
   private void Awake()
   {
      owner = GetComponent<Unit>();
      skillController = GetComponent<BaseSkillController>();
      InitializeSkillManager();
   }


   public void InitializeSkillManager()
   {
      foreach (SkillData skillData in selectedSkill)
      {
         Skill skill = new Skill
            (  
               skillData.skillType,
               skillData.selectCamp,
               skillData.selectType,
               skillData.mainSkillEffect,
               skillData.subSkillEffect,
               skillData.jobType,
               skillData.reuseMaxNumber,
               skillData.maxCost,
               skillData.skillIcon,
               skillData.SkillVFX,
               skillData.skillAnimation
               );
         skill.mainEffect.owner = owner;
         skill.subEffect.owner = owner;
         skill.mainEffect.Init();
         skill.subEffect.Init();
         skillController.skills.Add(skill);
      }
   }
   
   
   
}
