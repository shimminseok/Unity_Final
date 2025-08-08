using UnityEngine;


[CreateAssetMenu(fileName = "StrongMindPassive", menuName = "ScriptableObjects/PassiveSkill/StrongMindPassive", order = 0)]
public class StrongMindPassiveSo : PassiveSO, IPassiveEmotionDebuffReducer
{
    [SerializeField] private float debuffReduceValue;

    public override bool CanTrigger(BaseEmotion currentEmotion)
    {
        return currentEmotion.EmotionType == TriggerEmotion;
    }


    public void OnDebuffReducer(ref float debuffValue)
    {
        if (!CanTrigger(Owner.CurrentEmotion))
        {
            return;
        }

        float originalValue = debuffValue;

        debuffValue *= debuffReduceValue;
        Debug.Log($"디버프 확률 Before : {originalValue} After : {debuffValue}");
    }
}