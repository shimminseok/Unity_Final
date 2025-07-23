using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RewardAction", menuName = "ScriptableObjects/Tutorial/Actions/Reward", order = 0)]
public class RewardActionData : TutorialActionData
{
    public string rewardKey;

    private void OnEnable()
    {
        ActionType = TutorialActionType.Reward;
    }
}
