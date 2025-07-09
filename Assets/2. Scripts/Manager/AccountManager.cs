using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountManager : Singleton<AccountManager>
{
    public int Gold      { get; private set; } = 0;
    public int Opal      { get; private set; } = 3000;
    public int BestStage { get; private set; } = 1010101;

    public Dictionary<int, EntryDeckData> MyPlayerUnits = new Dictionary<int, EntryDeckData>();
    public Dictionary<int, ActiveSkillSO> MySkills = new Dictionary<int, ActiveSkillSO>();
    public event Action<int> OnGoldChanged;
    public event Action<int> OnOpalChanged;

    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 15;
        foreach (PlayerUnitSO playerUnitSo in TableManager.Instance.GetTable<PlayerUnitTable>().DataDic.Values)
        {
            AddPlayerUnit(playerUnitSo);
        }
    }

    private void Start()
    {
        //Test
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

    public void AddOpal(int amount)
    {
        Opal += amount;

        OnOpalChanged?.Invoke(Opal);
    }

    public void UseOpal(int amount)
    {
        if (Opal < amount)
        {
            return;
        }

        Opal -= amount;
        OnOpalChanged?.Invoke(Opal);
    }

    // Opal 사용 가능한지 보고 UI에서 판단
    public bool CanUseOpal(int amount)
    {
        return Opal >= amount;
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
        if (!MyPlayerUnits.TryGetValue(unit.ID, out EntryDeckData data))
        {
            data = new EntryDeckData(unit.ID);
            MyPlayerUnits[unit.ID] = data;
        }
        else
        {
            data.AddAmount();
        }
    }

    public void AddSkill(ActiveSkillSO skill, out bool isDuplicate)
    {
        if (MySkills.TryAdd(skill.ID, skill))
        {
            isDuplicate = false;
        }
        else
        {
            //TODO : 재화 돌려줌 -> 재화 돌려주는걱 가챠 시스템쪽에서 처리함
            isDuplicate = true;
        }
    }


    public EntryDeckData GetPlayerUnit(int id)
    {
        return MyPlayerUnits.GetValueOrDefault(id);
    }
}