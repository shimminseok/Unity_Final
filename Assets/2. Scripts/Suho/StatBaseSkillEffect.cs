using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatBaseSkillEffect // 개편 필요
{
    [HideInInspector] public Unit owner;
    public List<SkillEffectData> effects;

    public void AffectTargetWithSkill(IDamageable target) // 실질적으로 영향을 끼치는 부분
    {
        /*
         * 마음에 안드는 부분
         * 1. 스킬 한 번 쓰는데 effects 순회를 두 번
         * 2. foreach내부에서 if, else문
         * 3. effectType구분 / 데미지를 주는 방법 / 굳이 TranslateFrom.... 메서드가 필요한가? 등
         * 4. 실드 스킬의 경우는 일반적인 체력 계산과 다름
         */
        foreach (var result in effects)
        {
            var statusEffect = result.StatusEffect;
            statusEffect.Stat.Value = owner.StatManager.GetValue(result.ownerStatType) * result.weight;

            if (statusEffect.Stat.StatType == StatType.CurHp)
            {
                target.TakeDamage(statusEffect.Stat.Value); // TakeDamage 자체적으로 방어력으로 인한 데미지 감소 공식 존재하는 가?
            }
            else
            {
                target.Collider.GetComponent<Unit>().StatusEffectManager.ApplyEffect(BuffFactory.CreateBuff(statusEffect));
            }
        }
    }

    /*
     *  SkillEffectData를 StatusEffectData로 변환하는 메서드
     *  Value값은 owner가 가진 ownerStatType의 value * weight 값으로 정해진다.
     *  현재는 EffectType이 TurnBasedModifierBuff로 통일이지만, 이후 수정이 필요할 듯
     *  Duration은 lastTurn으로 정해진다. default = 1이고, 전체 턴 종료 시 바로 끝
     *
     */
    public List<StatusEffectData> TranslateFromSkillEffectDataToStatusEffectData()
    {
        List<StatusEffectData> resultEffect = new List<StatusEffectData>();
        foreach (SkillEffectData skillData in this.effects)
        {
            StatusEffectData sed = new StatusEffectData();
            sed.ID = skillData.ID;
            sed.EffectType = skillData.effectType;
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
        foreach (SkillEffectData skillData in this.effects)
        {
            skillData.StatusEffect = new StatusEffectData();
            {
                //불변 데이터
                skillData.StatusEffect.ID = skillData.ID;
                skillData.StatusEffect.EffectType = skillData.effectType;
                skillData.StatusEffect.Duration = skillData.lastTurn;
                skillData.StatusEffect.IsStackable = skillData.isStackable;
                skillData.StatusEffect.Stat = new StatData();
                skillData.StatusEffect.Stat.StatType = skillData.opponentStatType;
                /* 임시처리 */
                //바뀌는애
                skillData.StatusEffect.Stat.Value = owner.StatManager.GetValue(skillData.ownerStatType) * skillData.weight;
            }
        }
    }
}
//대미지 주는 class 하나
//디버프 주는 class 하나

[System.Serializable]
public class SkillEffectData
{
    public int ID;
    public StatType ownerStatType;    // 사용자에 의해 강해지거나 약해지는 사용자의 스텟타입 => ex) 남기사 실드스킬에서 남기사의 최대체력
    public float weight;              // 계수 => ownerStatType의 value값에서 몇 배율을 적용할 것인가, weight = 0.1이면 value * 0.1 
    public StatType opponentStatType; // 영향을 받는 스텟의 타입 => ex) 공격 스킬을 사용하면 상대의 curHP가 영향을 받음
    public StatusEffectType effectType;
    public int lastTurn = 1;        // 영향이 지속되는 턴 ( 턴 마다 지속되는 도트데미지, 디버프 등)
    public bool isStackable = true; // 디버프, 버프의 중첩가능여부
    public EmotionType emotionType;

    public StatusEffectData StatusEffect; //디버프
}