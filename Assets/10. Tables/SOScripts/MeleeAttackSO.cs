using UnityEngine;


[CreateAssetMenu(fileName = "NewMeleeAttack", menuName = "ScriptableObjects/AttackType/Melee", order = 0)]
public class MeleeAttackSo : CombatActionSo
{
    public override AttackDistanceType DistanceType => AttackDistanceType.Melee;

    public override void Execute(IAttackable attacker, IDamageable target)
    {
        IStatContext isc = attacker as IStatContext;
        if (isc == null) return;
        float baseAttackPow = isc.StatManager.GetValue(StatType.AttackPow);
        float finalValue    = baseAttackPow;

        if (attacker is PlayerUnitController player)
        {
            //여전사 패시브 (상성 관계없이 1.3배)
            if (player.PassiveSo is IPassiveEmotionDamageModifier modifier)
            {
                finalValue = modifier.ModifyEmotionDamage(finalValue);
                target.TakeDamage(finalValue);
                return;
            }

            if (player.PassiveSo is IPassiveAttackTrigger repeatPassive) //더블어택
                repeatPassive.OnAttack();
        }

        IDamageable attackerDamagable = attacker as IDamageable;
        if (attackerDamagable == null) return;
        float multiplier = EmotionAffinityManager.GetAffinityMultiplier(attackerDamagable.CurrentEmotion.EmotionType, target.CurrentEmotion.EmotionType);
        target.TakeDamage(finalValue * multiplier);
    }
}