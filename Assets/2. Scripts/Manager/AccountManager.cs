using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountManager : Singleton<AccountManager>
{
    public int Gold      { get; private set; } = 0;
    public int BestStage { get; private set; } = 1010101;

    public Dictionary<int, PlayerUnitData> MyPlayerUnits = new Dictionary<int, PlayerUnitData>();
    public event Action<int> OnGoldChanged;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        //Test
        foreach (PlayerUnitSO playerUnitSo in TableManager.Instance.GetTable<PlayerUnitTable>().DataDic.Values)
        {
            AddPlayerUnit(playerUnitSo);
        }
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

    public void AddPlayerUnit(PlayerUnitSO unit)
    {
        if (!MyPlayerUnits.TryGetValue(unit.ID, out PlayerUnitData data))
        {
            data = new PlayerUnitData(unit.ID);
            MyPlayerUnits[unit.ID] = data;
        }
        else
        {
            data.Amount++;
        }
    }
}