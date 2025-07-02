using System.Collections.Generic;
using UnityEngine;

public class SkillData
{
    public SkillData(ActiveSkillSO skillSo)
    {
        this.skillSo = skillSo;
        skillEffect = skillSo.skillEffect;
        reuseCount = skillSo.reuseMaxCount;
        coolDown = 0;
        coolTime = skillSo.coolTime;
    }

    public ActiveSkillSO skillSo;
    public StatBaseSkillEffect skillEffect;
    public int reuseCount;
    public int coolDown = 0;
    public int coolTime;

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