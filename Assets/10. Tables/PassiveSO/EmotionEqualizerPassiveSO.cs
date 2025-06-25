using UnityEngine;


[CreateAssetMenu(fileName = "EmotionEqualizerPassive", menuName = "ScriptableObject/PassiveSkill/EmotionEqualizerPassive", order = 0)]
public class EmotionEqualizerPassiveSo : PassiveSO, IEmotionDamageModifier
{
    private const float FixedMutiplier = 1.3f;

    public override bool CanTrigger(BaseEmotion currentEmotion)
    {
        return (currentEmotion.EmotionType == EmotionType.None);
    }

    public float ModifyEmotionDamage(float baseDamage)
    {
        return CanTrigger(Owner.CurrentEmotion) ? baseDamage * FixedMutiplier : baseDamage * 0.7f;
    }
}