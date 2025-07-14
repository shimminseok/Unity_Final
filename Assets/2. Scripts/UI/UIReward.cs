using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIReward : UIBase
{
    [SerializeField] private List<RewardSlot> rewardSlotList;

    private Action afterAction;
    private int index = 0;

    public void OpenRewardUI(Action action)
    {
        for (int i = index; i < rewardSlotList.Count; i++)
        {
            rewardSlotList[i].gameObject.SetActive(false);
        }

        afterAction = action;

        UIManager.Instance.Open(this);
    }

    public void AddReward(RewardSo rewardSo)
    {
        foreach (RewardData rewardData in rewardSo.RewardList)
        {
            if (index >= rewardSlotList.Count)
                break;

            rewardSlotList[index].SetRewardItem(rewardData);
            rewardSlotList[index].gameObject.SetActive(true);
            index++;
        }
    }

    public void CloseRewardUI()
    {
        UIManager.Instance.Close(this);
    }

    public override void Open()
    {
        base.Open();
    }

    public override void Close()
    {
        base.Close();
        index = 0;
        afterAction?.Invoke();
    }
}