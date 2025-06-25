using UnityEngine;


[CreateAssetMenu(fileName = "MagicMaster", menuName = "ScriptableObjects/PassiveSkill/MagicMasterPassive", order = 0)]
public class MagicMasterPassiveSo : PassiveSO
{
    public int addCost;

    public override bool CanTrigger(BaseEmotion currentEmotion)
    {
        return currentEmotion.EmotionType == TriggerEmotion;
    }


    public void AddCost()
    {
    }
}