using PixPlays.ElementalVFX;
using System;
using System.Net;
using UnityEngine;


public abstract class CombatActionSo : ScriptableObject, IAttackAction
{
    public abstract void               Execute(IAttackable attacker, IDamageable target);
    public abstract AttackDistanceType DistanceType { get; }
    public virtual  CombatActionSo     ActionSo     => this;
    
    [Header("무기로 때렸을 때의 타격음")]public SFXName HitSound;
    [Header("무기로 때릴 때 휘두르는/투사체를 날리는 사운드")]public SFXName AttackSound;


    public virtual void PlayCastVFX(IAttackable attacker, IDamageable target)
    {
        foreach (var data in attacker.SkillController.CurrentSkillData.skillSo.effect.skillEffectDatas)
        {
            VFXController.VFXListPlay(data.skillVFX,VFXType.Cast,VFXSpawnReference.Target, target as IEffectProvider,true);
            VFXController.VFXListPlay(data.skillVFX,VFXType.Cast,VFXSpawnReference.Caster, attacker as IEffectProvider,true);
        }
    }

    public virtual void PlaySFX(IAttackable attacker)
    {
        SFXName sfx = attacker.SkillController.CurrentSkillData.skillSo.SFX;
        if (sfx == SFXName.None) return;
        AudioManager.Instance.PlaySFX(sfx.ToString());
    }
    
    public virtual void PlayCastSFX(IAttackable attacker)
    {
        SFXName sfx = attacker.SkillController.CurrentSkillData.skillSo.CastSFX;
        if (sfx == SFXName.None) return;
        AudioManager.Instance.PlaySFX(sfx.ToString());
    }
    
}