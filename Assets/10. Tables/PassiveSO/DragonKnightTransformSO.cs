using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DragonKnightTransform", menuName = "ScriptableObjects/PassiveSkill/DragonKnightTransform", order = 0)]
public class DragonKnightTransformSO : PassiveSO, IPassiveTurnStartTrigger
{
    [Header("변신 조건")]
    public int RequiredStack;

    [Header("변신 시 적용되는 스탯 버프")]
    public List<StatData> TransformStats;


    private bool isTriggered;

    public override bool CanTrigger(BaseEmotion currentEmotion)
    {
        return RequiredStack <= currentEmotion.Stack && currentEmotion.EmotionType == TriggerEmotion;
    }

    public void OnTurnStart(Unit unit)
    {
        if (!CanTrigger(unit.CurrentEmotion))
        {
            if (isTriggered)
            {
                foreach (StatData buffStat in TransformStats)
                {
                    unit.StatManager.ApplyStatEffect(buffStat.StatType, buffStat.ModifierType, -buffStat.Value);
                }
            }

            isTriggered = false;
            return;
        }

        if (isTriggered)
            return;

        foreach (StatData buffStat in TransformStats)
        {
            unit.StatManager.ApplyStatEffect(buffStat.StatType, buffStat.ModifierType, buffStat.Value);
        }

        isTriggered = true;
    }
}