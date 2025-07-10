using UnityEngine;


[CreateAssetMenu(fileName = "NewRewardSo", menuName = "ScriptableObjects/Reward/Reward", order = 0)]
public class RewardSo : ScriptableObject
{
    public string Id;
    public int RewardGold;
    public int RewardOpal;

    //추후 Reward로 캐릭터를 지급해야할지 논의
}