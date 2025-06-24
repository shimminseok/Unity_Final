using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatBaseSkillEffect
{
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
       // target.TakeDamage(owner.StatManager.GetValue(standardStatType) * weight);
        
        
    }
}

[System.Serializable]
public class SkillEffectData
{
    public StatType ownerStatType;    // 사용자에 의해 강해지거나 약해지는 사용자의 스텟타입
    public float weight;               // 계수
    public StatType opponentStatType; // 영향을 받는 스텟의 타입
    public int lastTurn = 1; // 영향이 지속되는 턴 ( 턴 마다 지속되는 도트데미지, 디버프 등)
    


}
