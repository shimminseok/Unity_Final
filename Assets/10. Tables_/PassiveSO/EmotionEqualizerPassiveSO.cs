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
        float finalDamage = 0f;
        if (CanTrigger(Owner.CurrentEmotion))
        {
            finalDamage = baseDamage * FixedMutiplier;
        }
        else
        {
            finalDamage = baseDamage * 0.7f;
        }

        Debug.Log($"상성 무시 디버프 발동 : {CanTrigger(Owner.CurrentEmotion)}, Final Damage : {finalDamage}");
        return finalDamage;
    }
}