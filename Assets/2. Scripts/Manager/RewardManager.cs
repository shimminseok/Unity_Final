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

    private void Start()
    {
    }

    public void GiveReward(string id)
    {
        UIReward rewardUI = UIManager.Instance.GetUIComponent<UIReward>();

        RewardSo rewardSo = rewardTable.GetDataByID(id);
        if (rewardSo != null)
        {
            int slotIndex = 0;

            foreach (var reward in rewardSo.RewardList)
            {
                if (rewardHandlers.TryGetValue(reward.RewardType, out var handler))
                {
                    handler.Invoke(reward.Amount);
                }
            }
        }

        rewardUI.OpenRewardUI(rewardSo, () =>
        {
            LoadSceneManager.Instance.LoadScene("DeckBuildingScene");
        });
    }
}