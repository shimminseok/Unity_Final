using UnityEngine;


[CreateAssetMenu(fileName = "AllyDeathPassive", menuName = "ScriptableObjects/PassiveSkill/AllyDeathPassive", order = 0)]
public class AllyDeathPassiveSo : PassiveSO, IPassiveAllyDeathTrigger
{
    public int emotionStackPerAllyDeath;

    public override bool CanTrigger(BaseEmotion currentEmotion)
    {
        return currentEmotion.EmotionType == TriggerEmotion;
    }

    public void OnAllyDead()
    {
        if (!CanTrigger(Owner.CurrentEmotion))
        {
            return;
        }

        Owner.CurrentEmotion.AddStack(Owner, emotionStackPerAllyDeath);
    }

    public override void ExecutePassive()
    {
        OnAllyDead();
    }
}