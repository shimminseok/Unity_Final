using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIReward : UIBase
{
    [SerializeField] private List<RewardSlot> rewardSlotList;

    Action afterAction;

    public void OpenRewardUI(RewardSo rewardSo, Action action)
    {
        UIManager.Instance.Open(this);


        int slotIndex = 0;

        foreach (var reward in rewardSo.RewardList)
        {
            if (slotIndex >= rewardSlotList.Count)
                break;

            rewardSlotList[slotIndex].SetRewardItem(reward); // reward 데이터에 따라 슬롯 세팅
            slotIndex++;
        }

        for (int i = slotIndex; i < rewardSlotList.Count; i++)
        {
            rewardSlotList[i].gameObject.SetActive(false);
        }

        afterAction = action;
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
        afterAction?.Invoke();
    }
}