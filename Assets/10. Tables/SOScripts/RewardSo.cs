using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewRewardSo", menuName = "ScriptableObjects/Reward/Reward", order = 0)]
public class RewardSo : ScriptableObject
{
    public string Id;

    public List<RewardData> RewardList;
    //추후 Reward로 캐릭터를 지급해야할지 논의
}

[Serializable]
public class RewardData
{
    public RewardType RewardType;
    public int Amount;
    public string ItemId;
}