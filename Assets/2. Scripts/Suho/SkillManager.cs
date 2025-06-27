using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
   public List<ActiveSkillSO> selectedSkill;
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
               skillData.mainEffect.owner = owner;
               skillData.subEffect.owner = owner;
               skillData.mainEffect.Init();
               skillData.subEffect.Init();
         skillController.skills.Add(skillData);
      }
   }
   
   
   
}
