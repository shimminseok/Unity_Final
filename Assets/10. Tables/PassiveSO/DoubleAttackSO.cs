using UnityEngine;


[CreateAssetMenu(fileName = "DoubleAttack", menuName = "ScriptableObjects/PassiveSkill/DoubleAttack", order = 0)]
public class DoubleAttack : PassiveSO, IPassiveAttackTrigger
{
    public int RequiredStack;

    public override bool CanTrigger(BaseEmotion currentEmotion)
    {
        return currentEmotion.EmotionType == TriggerEmotion && currentEmotion.Stack >= RequiredStack;
    }

    public void OnAttack()
    {
        if (!CanTrigger(Owner.CurrentEmotion))
            return;

        float baseDamage = Owner.StatManager.GetValue(StatType.AttackPow);


        float mutiplier   = EmotionAffinityManager.GetAffinityMultiplier(Owner.CurrentEmotion.EmotionType, Owner.Target.CurrentEmotion.EmotionType);
        float finalDamage = baseDamage * mutiplier;

        // Owner.Attack();
        Owner.Target.TakeDamage(finalDamage);
    }
}