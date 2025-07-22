using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewRewardSo", menuName = "ScriptableObjects/Reward/Reward", order = 0)]
public class RewardSo : ScriptableObject
{
    public string Id;

    public List<RewardData> RewardList;
}

[Serializable]
public class RewardData
{
    public RewardType RewardType;
    public int Amount;
    public string ItemId;
}