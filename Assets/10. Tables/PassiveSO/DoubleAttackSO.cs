using UnityEngine;


[CreateAssetMenu(fileName = "DoubleAttack", menuName = "ScriptableObjects/PassiveSkill/DoubleAttack", order = 0)]
public class Double : PassiveSO, IPassiveAttackTrigger
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

        //Test
        if (Owner.Target == null)
        {
            Debug.Log(baseDamage * EmotionAffinityManager.GetAffinityMultiplier(Owner.CurrentEmotion.EmotionType, GameManager.Instance.testEmotion));
            return;
        }

        float mutiplier   = EmotionAffinityManager.GetAffinityMultiplier(Owner.CurrentEmotion.EmotionType, Owner.Target.CurrentEmotion.EmotionType);
        float finalDamage = baseDamage * mutiplier;


        Owner.Target.TakeDamage(finalDamage);
    }
}