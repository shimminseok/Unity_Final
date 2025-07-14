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
    public string projectilePoolID;
    public List<StatBaseDamageEffect> damageEffects;
    public List<StatBaseBuffSkillEffect> buffEffects;
    public List<VFXData> skillVFX;
    
    public void AffectTargetWithSkill(IDamageable target) // 실질적으로 영향을 끼치는 부분
    {
        foreach (var result in buffEffects)
        {
            VFXController.VFXListPlay(skillVFX,VFXType.Hit,VFXSpawnReference.Target, target as IEffectProvider,true);
            VFXController.VFXListPlay(skillVFX,VFXType.Hit,VFXSpawnReference.Caster, owner as IEffectProvider,true);
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

}




