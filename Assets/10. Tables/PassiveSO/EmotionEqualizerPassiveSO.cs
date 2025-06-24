using UnityEngine;


[CreateAssetMenu(fileName = "EmotionEqualizerPassive", menuName = "ScriptableObject/PassiveSkill/EmotionEqualizerPassive", order = 0)]
public class EmotionEqualizerPassiveSO : PassiveSO, IEmotionDamageModifier
{
    private const float fixedMutiplier = 1.3f;

    public override bool CanTrigger(BaseEmotion currentEmotion)
    {
        return (currentEmotion.EmotionType == EmotionType.None);
    }

    public float ModifyEmotionDamage(float baseDamage)
    {
        return CanTrigger(Owner.CurrentEmotion) ? baseDamage * fixedMutiplier : EmotionAffinityManager.GetAffinityMultiplier(Owner.CurrentEmotion.EmotionType, Owner.Target.CurrentEmotion.EmotionType);
    }
}