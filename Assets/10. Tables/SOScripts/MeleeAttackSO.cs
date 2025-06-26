using UnityEngine;


[CreateAssetMenu(fileName = "NewMeleeAttack", menuName = "ScriptableObjects/AttackType/Melee", order = 0)]
public class MeleeAttackSO : AttackTypeSO
{
    public override void Attack(Unit attacker)
    {
        float baseAttackPow = attacker.StatManager.GetValue(StatType.AttackPow);
        float finalValue    = baseAttackPow;

        if (attacker is PlayerUnitController player)
        {
            if (player.passiveSo is IPassiveEmotionDamageModifier modifier) //여전사 패시브 (상성 관계없이 1.3배)
                finalValue = modifier.ModifyEmotionDamage(finalValue);
            else if (player.passiveSo is IPassiveAttackTrigger repeatPassive) //더블어택
                repeatPassive.OnAttack();
        }

        float mutiplier = 0f;
        mutiplier = EmotionAffinityManager.GetAffinityMultiplier(attacker.CurrentEmotion.EmotionType, attacker.Target.CurrentEmotion.EmotionType);
        finalValue *= mutiplier;
        attacker.Target.TakeDamage(finalValue);
    }
}