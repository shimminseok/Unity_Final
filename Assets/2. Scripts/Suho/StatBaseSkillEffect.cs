using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class StatBaseSkillEffect
{
    [HideInInspector] public Unit owner;
    public List<SkillEffectData> skillEffectDatas;
    public void Init()
    {
        foreach (SkillEffectData effect in this.skillEffectDatas)
        {
            effect.owner = this.owner;
            foreach (var buffSkillEffect in effect.buffEffects)
            {
                buffSkillEffect.StatusEffect = new StatusEffectData();
                {
                    //불변 데이터
                    buffSkillEffect.StatusEffect.EffectType = buffSkillEffect.statusEffectType;
                    buffSkillEffect.StatusEffect.Duration = buffSkillEffect.lastTurn;
                    buffSkillEffect.StatusEffect.Stat = new StatData();
                    buffSkillEffect.StatusEffect.Stat.StatType = buffSkillEffect.opponentStatType;
                    buffSkillEffect.StatusEffect.Stat.ModifierType = buffSkillEffect.modifierType;
                }
            }
        }
    }
}