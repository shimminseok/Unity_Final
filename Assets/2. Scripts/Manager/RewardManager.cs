using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : Singleton<RewardManager>
{
    private RewardTable rewardTable;

    protected override void Awake()
    {
        base.Awake();
        if (isDuplicated)
            return;
        rewardTable = TableManager.Instance.GetTable<RewardTable>();
    }

    private void Start()
    {
    }

    public void GiveReward(string id)
    {
        RewardSo reward = rewardTable.GetDataByID(id);
        if (reward != null)
        {
            if (reward.RewardGold > 0)
            {
                AccountManager.Instance.AddGold(reward.RewardGold);
            }

            if (reward.RewardOpal > 0)
            {
                AccountManager.Instance.AddOpal(reward.RewardOpal);
            }
        }
    }
}