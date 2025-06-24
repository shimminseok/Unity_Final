using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
   public List<SkillData> selectedSkill;
   public BaseSkillController skillController;

   private void Awake()
   {
      InitializeSkillManager();
   }


   public void InitializeSkillManager()
   {
      foreach (SkillData skillData in selectedSkill)
      {
         Skill skill = new Skill
            (  skillData.selectCamp,
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
         skillController.skills.Add(skill);
      }
   }
   
   
   
}
