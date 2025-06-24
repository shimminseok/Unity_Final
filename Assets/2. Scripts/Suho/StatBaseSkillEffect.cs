using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatBaseSkillEffect
{
    public Unit owner;
    public StatType standardStatType;
    public float weight;
    public List<StatusEffectData> effects;

    public void UseSkill(IDamageable target)
    {
        // foreach (var effect in effects)
        // {
        //     effect.Stat.Value = owner.StatManager.GetValue(standardStatType) * weight;
        //     StatusEffect affect = BuffFactory.CreateBuff(effect);
        // }
        target.TakeDamage(owner.StatManager.GetValue(standardStatType) * weight);
        
        
    }
}
