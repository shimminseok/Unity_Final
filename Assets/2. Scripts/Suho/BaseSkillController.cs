using System.Collections.Generic;
using UnityEngine;

/*
 * 
 */
public abstract class BaseSkillController : MonoBehaviour
{
   public List<Skill> skills = new List<Skill>();
   public Skill currentSkill;
   public IDamageable mainTarget;
   public List<IDamageable> subTargets = new List<IDamageable>();
   public abstract void SelectTargets(IDamageable mainTarget);
   public int generateCost = 1; // 턴 종료 시 cost각 스킬 코스트 추가 defalut 값 = 1
   public abstract void UseSkill();
   public abstract void EndTurn();
}
