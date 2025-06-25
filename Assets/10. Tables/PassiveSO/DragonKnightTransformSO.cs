using UnityEngine;


[CreateAssetMenu(fileName = "DragonKnightTransform", menuName = "ScriptableObjects/PassiveSkill/DragonKnightTransform", order = 0)]
public class DragonKnightTransformSO : PassiveSO, ITurnStartTrigger
{
    public int RequiredStack;

    public override bool CanTrigger(BaseEmotion currentEmotion)
    {
        return RequiredStack >= currentEmotion.Stack && currentEmotion.EmotionType == EmotionType.Anger;
    }

    public void OnTurnStart(Unit unit)
    {
        if (!CanTrigger(unit.CurrentEmotion))
            return;
    }
}