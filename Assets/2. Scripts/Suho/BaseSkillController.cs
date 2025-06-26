using System;
using System.Collections.Generic;
using UnityEngine;

/*
 * 
 */
public abstract class BaseSkillController : MonoBehaviour
{
   public List<Skill> skills = new List<Skill>();
   public Skill currentSkill;
   public Unit mainTarget;
   public List<Unit> subTargets = new List<Unit>();


   public abstract void SelectTargets(Unit mainTarget);
   public int generateCost = 1; // 턴 종료 시 cost각 스킬 코스트 추가 defalut 값 = 1
   public abstract void UseSkill();
   public abstract void EndTurn();
}
