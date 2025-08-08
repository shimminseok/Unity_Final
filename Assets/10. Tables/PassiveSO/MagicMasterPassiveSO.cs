using UnityEngine;


[CreateAssetMenu(fileName = "MagicMaster", menuName = "ScriptableObjects/PassiveSkill/MagicMasterPassive", order = 0)]
public class MagicMasterPassiveSo : PassiveSO, IPassiveTurnEndTrigger
{
    public int addCost;

    public override bool CanTrigger(BaseEmotion currentEmotion)
    {
        return currentEmotion.EmotionType == TriggerEmotion;
    }

    public void OnTurnEnd(Unit unit)
    {
        Owner.SkillController.generateCost = CanTrigger(Owner.CurrentEmotion) ? addCost : 1;
    }
}