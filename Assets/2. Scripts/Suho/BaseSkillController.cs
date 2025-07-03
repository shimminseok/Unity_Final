using System;
using System.Collections.Generic;
using UnityEngine;

/*
 *
 */
[RequireComponent(typeof(SkillManager))]
public abstract class BaseSkillController : MonoBehaviour
{
    public List<SkillData> skills = new List<SkillData>();
    public SkillData CurrentSkillData;
    public Unit mainTarget;
    public List<Unit> targets = new List<Unit>();
    public int generateCost = 1; // 턴 종료 시 cost각 스킬 코스트 추가 defalut 값 = 1


    public SkillManager SkillManager { get; private set; }


    public void Initialize(SkillManager manager)
    {
        SkillManager = manager;
    }

    public abstract void SelectTargets(Unit mainTarget);
    public abstract void UseSkill();
    public abstract void EndTurn();

    public virtual void ChangeSkill(int index) { }

    public int GetSkillIndex(SkillData skill)
    {
        return skills.FindIndex(s => s == skill);
    }
}