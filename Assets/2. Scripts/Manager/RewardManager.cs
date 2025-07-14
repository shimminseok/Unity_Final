using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : Singleton<RewardManager>
{
    private RewardTable rewardTable;


    private readonly Dictionary<RewardType, Action<int>> rewardHandlers = new()
    {
        { RewardType.Gold, amount => AccountManager.Instance.AddGold(amount) }, { RewardType.Opal, amount => AccountManager.Instance.AddOpal(amount) },
        // 추후 추가 가능: Character, Item 등
    };

    protected override void Awake()
    {
        base.Awake();
        if (isDuplicated)
            return;
        rewardTable = TableManager.Instance.GetTable<RewardTable>();
    }

    private UIReward rewardUI = UIManager.Instance.GetUIComponent<UIReward>();

    private void Start()
    {
    }

    public void GiveReward(string id)
    {
        RewardSo rewardSo = rewardTable.GetDataByID(id);
        if (rewardSo == null)
            return;

        foreach (RewardData reward in rewardSo.RewardList)
        {
            if (rewardHandlers.TryGetValue(reward.RewardType, out var handler))
            {
                handler.Invoke(reward.Amount);
            }
        }
    }

    public void AddReward(string id)
    {
        RewardSo rewardSo = rewardTable.GetDataByID(id);
        if (rewardSo == null)
            return;

        rewardUI.AddReward(rewardSo);
        GiveReward(id);
    }

    /// <summary>
    /// 여러곳에서 보상을 추가 후 마지막에 호출하여 보상 UI를 Open
    /// </summary>
    /// <param name="onComplete"></param>
    public void GiveRewardAndOpenUI(Action onComplete = null)
    {
        rewardUI.OpenRewardUI(onComplete);
    }
}