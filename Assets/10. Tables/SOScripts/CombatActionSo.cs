using PixPlays.ElementalVFX;
using System;
using UnityEngine;


public abstract class CombatActionSo : ScriptableObject, IAttackAction
{
    public abstract void               Execute(IAttackable attacker, IDamageable target);
    public abstract AttackDistanceType DistanceType { get; }
    public virtual  CombatActionSo     ActionSo     => this;

    public virtual void PlayVFX(IAttackable attacker, IDamageable target)
    {
        foreach (var data in attacker.SkillController.CurrentSkillData.skillSo.effect.skillEffectDatas)
        {
            VFXController.VFXListPlay(data.skillVFX,VFXType.Cast,VFXSpawnReference.Target, target as IEffectProvider,true);
            VFXController.VFXListPlay(data.skillVFX,VFXType.Cast,VFXSpawnReference.Caster, attacker as IEffectProvider,true);
        }
    }

    public virtual void PlaySFX(IAttackable attacker)
    {
        foreach (SFXName sfx in attacker.SkillController.CurrentSkillData.skillSo.SFX)
        {
            AudioManager.Instance.PlaySFX(sfx.ToString());
        }
    }
    
}