using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIReward : UIBase
{
    [SerializeField] private List<InventorySlot> inventorySlotList;

    private Action afterAction;
    private int index = 0;

    public void OpenRewardUI(Action action)
    {
        for (int i = index; i < inventorySlotList.Count; i++)
        {
            inventorySlotList[i].gameObject.SetActive(false);
        }

        afterAction = action;

        UIManager.Instance.Open(this);
    }

    public void AddReward(RewardSo rewardSo)
    {
        foreach (RewardData rewardData in rewardSo.RewardList)
        {
            if (index >= inventorySlotList.Count)
                break;

            inventorySlotList[index].Initialize(rewardData);
            inventorySlotList[index].gameObject.SetActive(true);
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