using UnityEngine;


[CreateAssetMenu(fileName = "DoubleAttack", menuName = "ScriptableObjects/PassiveSkill/DoubleAttack", order = 0)]
public class DoubleAttack : PassiveSO, IPassiveAttackTrigger
{
    public int RequiredStack;

    public override bool CanTrigger(BaseEmotion currentEmotion)
    {
        return currentEmotion.EmotionType == TriggerEmotion && currentEmotion.Stack >= RequiredStack;
    }

    public void OnAttack(float value)
    {
        if (!CanTrigger(Owner.CurrentEmotion))
        {
            return;
        }


        IDamageable finalTarget = Owner.IsCounterAttack ? Owner.CounterTarget : Owner.Target;

        float mutiplier   = EmotionAffinityManager.GetAffinityMultiplier(Owner.CurrentEmotion.EmotionType, finalTarget.CurrentEmotion.EmotionType);
        float finalDamage = value * mutiplier;

        finalTarget.TakeDamage(finalDamage);

        Debug.Log("더블 어택 성공");
    }
}