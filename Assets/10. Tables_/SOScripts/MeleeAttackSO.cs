using UnityEngine;


[CreateAssetMenu(fileName = "NewMeleeAttack", menuName = "ScriptableObjects/AttackType/Melee", order = 0)]
public class MeleeAttackSo : CombatActionSo
{
    public override AttackDistanceType DistanceType => AttackDistanceType.Melee;

    public override void Execute(IAttackable attacker, IDamageable target)
    {
        float baseAttackPow = attacker.StatManager.GetValue(StatType.AttackPow);
        float finalValue    = baseAttackPow;
        PlaySFX(attacker);
        bool isCritical = Random.value < attacker.StatManager.GetValue(StatType.CriticalRate);
        if (isCritical)
        {
            float critBouns = attacker.StatManager.GetValue(StatType.CriticalDam);
            finalValue *= 2.0f + critBouns;
        }

        if (attacker is PlayerUnitController player)
        {
            //여전사 패시브 (상성 관계없이 1.3배)
            if (player.PassiveSo is IPassiveEmotionDamageModifier modifier)
            {
                finalValue = modifier.ModifyEmotionDamage(finalValue);
                target.TakeDamage(finalValue, StatModifierType.Base, isCritical);
                return;
            }

            if (player.PassiveSo is IPassiveAttackTrigger repeatPassive) //더블어택
            {
                repeatPassive.OnAttack(finalValue);
            }
        }

        float multiplier = EmotionAffinityManager.GetAffinityMultiplier(attacker.CurrentEmotion.EmotionType, target.CurrentEmotion.EmotionType);
        target.TakeDamage(finalValue * multiplier, StatModifierType.Base, isCritical);
    }

    public override void PlaySFX(IAttackable attacker)
    {
        AudioManager.Instance.PlaySFX(AttackSound.ToString());
        AudioManager.Instance.PlaySFX(HitSound.ToString());
    }
}