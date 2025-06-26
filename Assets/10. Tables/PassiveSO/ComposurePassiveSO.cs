using UnityEngine;


[CreateAssetMenu(fileName = "ComposurePassiveSO", menuName = "ScriptableObjects/PassiveSkill/ComposurePassiveSO", order = 0)]
public class ComposurePassiveSo : PassiveSO
{
    public override bool CanTrigger(BaseEmotion currentEmotion)
    {
        return currentEmotion.EmotionType == TriggerEmotion;
    }

    public float ComposureValue(float hitRate)
    {
        return CanTrigger(Owner.CurrentEmotion) ? 1f : hitRate;
    }
}