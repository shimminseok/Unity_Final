using UnityEngine;


[CreateAssetMenu(fileName = "SadKidPassive", menuName = "ScriptableObjects/PassiveSkill/SadKidPassive", order = 0)]
public class SadKidPassiveSo : PassiveSO, IPassiveChangeEmotionTrigger
{
    public int emotionStackPerChangeEmotion;

    public override bool CanTrigger(BaseEmotion currentEmotion)
    {
        return currentEmotion.EmotionType == TriggerEmotion;
    }

    public void OnChangeEmotion()
    {
        if (!CanTrigger(Owner.CurrentEmotion))
            return;

        Owner.CurrentEmotion.AddStack(Owner, emotionStackPerChangeEmotion);
    }
}