using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class SkillEffectData
{
    [HideInInspector] public Unit owner;
    public SelectCampType selectCamp;
    public SelectTargetType selectTarget;
    public GameObject projectilePrefab;
    public List<StatBaseDamageEffect> damageEffects;
    public List<StatBaseBuffSkillEffect> buffEffects;
    public List<VFXData> skillVFX;
    
    public void AffectTargetWithSkill(Unit target) // 실질적으로 영향을 끼치는 부분
    {
        InitVFX(target);
        foreach (var result in buffEffects)
        {
            var statusEffect = result.StatusEffect;
            statusEffect.Stat.Value = owner.StatManager.GetValue(result.ownerStatType) * result.weight + result.value;
            target.StatusEffectManager.ApplyEffect(BuffFactory.CreateBuff(statusEffect));
            target.ChangeEmotion(result.emotionType);
        }


        foreach (var result in damageEffects)
        {
            target.ExecuteCoroutine(result.DamageEffectCoroutine(target, owner));
        }
    }

    public void InitVFX(Unit unit)
    {
        foreach (var vfx in skillVFX)
        {
            vfx.Attacker = owner;
            vfx.Target = unit;
            if (vfx.type == VFXType.Hit)
            {
                
            }
        }
    }
}




