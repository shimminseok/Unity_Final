using UnityEngine;


[CreateAssetMenu(fileName = "NewMeleeAttack", menuName = "ScriptableObjects/AttackType/Melee", order = 0)]
public class MeleeAttackSo : CombatActionSo
{
    public override AttackDistanceType DistanceType => AttackDistanceType.Melee;

    public override void Execute(Unit attacker, IDamageable target)
    {
        float baseAttackPow = attacker.StatManager.GetValue(StatType.AttackPow);
        float finalValue    = baseAttackPow;

        if (attacker is PlayerUnitController player)
        {
            //여전사 패시브 (상성 관계없이 1.3배)
            if (player.passiveSo is IPassiveEmotionDamageModifier modifier)
            {
                finalValue = modifier.ModifyEmotionDamage(finalValue);
                target.TakeDamage(finalValue);
                return;
            }

            if (player.passiveSo is IPassiveAttackTrigger repeatPassive) //더블어택
                repeatPassive.OnAttack();
        }

        float multiplier = EmotionAffinityManager.GetAffinityMultiplier(attacker.CurrentEmotion.EmotionType, target.CurrentEmotion.EmotionType);
        target.TakeDamage(finalValue * multiplier);
    }
}