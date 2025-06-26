using UnityEngine;


[CreateAssetMenu(fileName = "EmotionEqualizerPassive", menuName = "ScriptableObjects/PassiveSkill/EmotionEqualizerPassive", order = 0)]
public class PassiveEmotionEqualizerPassiveSo : PassiveSO, IPassiveEmotionDamageModifier
{
    private const float FixedMutiplier = 1.3f;

    public override bool CanTrigger(BaseEmotion currentEmotion)
    {
        return currentEmotion.EmotionType == TriggerEmotion;
    }

    public float ModifyEmotionDamage(float baseDamage)
    {
        return CanTrigger(Owner.CurrentEmotion) ? baseDamage * FixedMutiplier : baseDamage * 0.7f;
    }
}