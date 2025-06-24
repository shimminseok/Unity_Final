using UnityEngine;


[CreateAssetMenu(fileName = "NewMeleeAttack", menuName = "ScriptableObject/AttackType/Melee", order = 0)]
public class MeleeAttackSO : AttackTypeSO
{
    public override void Attack()
    {
        float baseAttackPow = Owner.StatManager.GetValue(StatType.AttackPow);
        float finalValue    = baseAttackPow;

        if (Owner is PlayerUnitController player)
        {
            if (player.PassiveSo is IEmotionDamageModifier modifier)
                finalValue = modifier.ModifyEmotionDamage(finalValue);
            else if (player.PassiveSo is IAttackRepeat repeatPassive)
                repeatPassive.OnAttackRepeat();
        }

        float mutiplier = 0f;
        //Test
        if (Owner.Target == null)
        {
            mutiplier = EmotionAffinityManager.GetAffinityMultiplier(Owner.CurrentEmotion.EmotionType, GameManager.Instance.testEmotion);
            Debug.Log(finalValue * mutiplier);
        }
        else
        {
            mutiplier = EmotionAffinityManager.GetAffinityMultiplier(Owner.CurrentEmotion.EmotionType, Owner.Target.CurrentEmotion.EmotionType);
            finalValue *= mutiplier;
            Owner.Target.TakeDamage(finalValue);
        }
    }
}