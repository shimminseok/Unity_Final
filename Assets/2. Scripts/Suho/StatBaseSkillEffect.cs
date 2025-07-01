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
    public List<StatBaseDamageEffect> damageEffects;
    public List<StatBaseBuffSkillEffect> buffEffects;
    public void AffectTargetWithSkill(Unit target) // 실질적으로 영향을 끼치는 부분
    {
            foreach (var result in buffEffects)
            {
                var statusEffect = result.StatusEffect;
                statusEffect.Stat.Value = owner.StatManager.GetValue(result.ownerStatType) * result.weight + result.value;
                target.StatusEffectManager.ApplyEffect(BuffFactory.CreateBuff(statusEffect));
                target.ChangeEmotion(result.emotionType);  
            }


            foreach (var result in damageEffects)
            {
                target.ExecuteCoroutine(result.DamageEffectCoroutine(target,owner));
            }
    }
}

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
                    buffSkillEffect.StatusEffect.ID = buffSkillEffect.ID;
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
//대미지 주는 class 하나
[System.Serializable]
public class StatBaseDamageEffect
{
    [Header("공격 횟수")]
    public int attackCount = 1;  // 공격 횟수 => weight * attackCount = 실제 weight
    private float attackDelay = 0.2f;
    [Header("어떤 스텟을 기준으로 데미지를 줄까")]
    public StatType ownerStatType;    // 사용자에 의해 강해지거나 약해지는 사용자의 스텟타입 => ex) 남기사 실드스킬에서 남기사의 최대체력
    [Header("기준스텟에 대한 가중치와 고정 값")]
    public float weight;              // 계수 => ownerStatType의 value값에서 몇 배율을 적용할 것인가, weight = 0.1이면 value * 0.1 
    public float value;                 // 기본 고정 값 : weight와 관계없이 고정으로 부여할 수 있는 값

    // 데미지 계산 식 => value + (weight * attackCount * statValue) 
    public void DamageEffect(Unit target, Unit owner)
    {
        float finalValue = value + (weight * owner.StatManager.GetValue(ownerStatType));
        target.TakeDamage(finalValue);
    }

    public IEnumerator DamageEffectCoroutine(Unit target, Unit owner)
    {
        int currentCount = 0;
        
        while (currentCount < attackCount)
        {
            DamageEffect(target, owner);
            currentCount++;
            yield return new WaitForSeconds(attackDelay);
        }
        currentCount = 0;
        yield return null;
    }
}

//디버프 주는 class 하나
[System.Serializable]
public class StatBaseBuffSkillEffect
{
    public int ID;
    [Header("영향을 주는 스텟")]
    public StatType ownerStatType;    // 사용자에 의해 강해지거나 약해지는 사용자의 스텟타입 => ex) 남기사 실드스킬에서 남기사의 최대체력
    [Header("가중치와 고정값")]
    public float weight;              // 계수 => ownerStatType의 value값에서 몇 배율을 적용할 것인가, weight = 0.1이면 value * 0.1 
    public float value;                 // 기본 고정 값 : weight와 관계없이 고정으로 부여할 수 있는 값
    [Header("영향을 받는 스텟")]
    public StatType opponentStatType; // 영향을 받는 스텟의 타입 => ex) 공격 스킬을 사용하면 상대의 curHP가 영향을 받음
    [Header("지속 턴")]
    public int lastTurn = 1;        // 영향이 지속되는 턴 ( 턴 마다 지속되는 도트데미지, 디버프 등)
    [Header("받는 디버프의 타입")]
    public StatusEffectType statusEffectType;
    public StatModifierType modifierType;
    [Header("대상의 감정을 바꾸는 공격")]
    public EmotionType emotionType;
    [HideInInspector]public StatusEffectData StatusEffect; //디버프
}
