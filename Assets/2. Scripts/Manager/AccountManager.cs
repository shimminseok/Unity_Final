using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountManager : Singleton<AccountManager>
{
    public int Gold      { get; private set; } = 0;
    public int BestStage { get; private set; } = 1010101;

    public event Action<int> OnGoldChanged;

    protected override void Awake()
    {
        base.Awake();
    }

    public void AddGold(int amount)
    {
        Gold += amount;

        OnGoldChanged?.Invoke(Gold);
    }

    public void UseGold(int amount, out bool result)
    {
        if (Gold < amount)
        {
            result = false;
            return;
        }

        Gold -= amount;
        result = true;
        OnGoldChanged?.Invoke(Gold);
    }

    public void UpdateBestStage(int stage)
    {
    }

    public void SetBestStage(int stage)
    {
        BestStage = stage;
    }
}