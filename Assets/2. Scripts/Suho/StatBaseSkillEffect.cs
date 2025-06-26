using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatBaseSkillEffect // 개편 필요
{
    [HideInInspector] public Unit owner;
    public List<StatBaseDamageEffect> damageEffects;
    public List<StatBaseBuffSkillEffect> buffEffects;

    public void AffectTargetWithSkill(Unit target) // 실질적으로 영향을 끼치는 부분
    {

        foreach (var result in buffEffects)
        {
            var statusEffect = result.StatusEffect;
            statusEffect.Stat.Value = owner.StatManager.GetValue(result.ownerStatType) * result.weight;
            target.StatusEffectManager.ApplyEffect(BuffFactory.CreateBuff(statusEffect));
        }

        foreach (var result in damageEffects)
        {
            
        }
        
    }


    public List<StatusEffectData> InstantiateBuffEffects()
    {
        List<StatusEffectData> resultEffect = new List<StatusEffectData>();
        foreach (StatBaseBuffSkillEffect skillData in this.buffEffects)
        {
            StatusEffectData sed = new StatusEffectData();
            sed.ID = skillData.ID;
            sed.Duration = skillData.lastTurn;
            sed.IsStackable = skillData.isStackable;
            sed.Stat = new StatData();
            sed.Stat.StatType = skillData.opponentStatType;
            /* 임시처리 */
            sed.Stat.Value = owner.StatManager.GetValue(skillData.ownerStatType) * skillData.weight;
            sed.TickInterval = 0;
            resultEffect.Add(sed);
        }

        return resultEffect;
    }

    public void Init()
    {
        foreach (StatBaseBuffSkillEffect skillData in this.buffEffects)
        {
            skillData.StatusEffect = new StatusEffectData();
            {
                //불변 데이터
                skillData.StatusEffect.ID = skillData.ID;
                skillData.StatusEffect.EffectType = StatusEffectType.TurnBasedModifierBuff;
                skillData.StatusEffect.Duration = skillData.lastTurn;
                skillData.StatusEffect.IsStackable = skillData.isStackable;
                skillData.StatusEffect.Stat = new StatData();
                skillData.StatusEffect.Stat.StatType = skillData.opponentStatType;
                //바뀌는애
                skillData.StatusEffect.Stat.Value = owner.StatManager.GetValue(skillData.ownerStatType) * skillData.weight;
            }
        }
    }
}
//대미지 주는 class 하나
[System.Serializable]
public class StatBaseDamageEffect
{
    public int attackCount;  // 공격 횟수 => weight * attackCount = 실제 weight
    private int currentCount = 0;
    private float attackDelay = 0.5f;
    public StatType ownerStatType;    // 사용자에 의해 강해지거나 약해지는 사용자의 스텟타입 => ex) 남기사 실드스킬에서 남기사의 최대체력
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
    public StatType ownerStatType;    // 사용자에 의해 강해지거나 약해지는 사용자의 스텟타입 => ex) 남기사 실드스킬에서 남기사의 최대체력
    public float weight;              // 계수 => ownerStatType의 value값에서 몇 배율을 적용할 것인가, weight = 0.1이면 value * 0.1 
    public StatType opponentStatType; // 영향을 받는 스텟의 타입 => ex) 공격 스킬을 사용하면 상대의 curHP가 영향을 받음
    public float value;                 // 기본 고정 값 : weight와 관계없이 고정으로 부여할 수 있는 값
    public int lastTurn = 1;        // 영향이 지속되는 턴 ( 턴 마다 지속되는 도트데미지, 디버프 등)
    public bool isStackable = true; // 디버프, 버프의 중첩가능여부
    public EmotionType emotionType;
    [HideInInspector]public StatusEffectData StatusEffect; //디버프
}
