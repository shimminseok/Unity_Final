using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountManager : Singleton<AccountManager>
{
    public int Gold      { get; private set; } = 0;
    public int BestStage { get; private set; } = 1010101;

    public Dictionary<int, EntryDeckData> MyPlayerUnits = new Dictionary<int, EntryDeckData>();
    public Dictionary<int, ActiveSkillSO> MySkills = new Dictionary<int, ActiveSkillSO>();
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
        if (!MyPlayerUnits.TryGetValue(unit.ID, out EntryDeckData data))
        {
            data = new EntryDeckData() { characterSO = unit };
            MyPlayerUnits[unit.ID] = data;
        }
        else
        {
            data.AddAmount();
        }
    }

    public void AddSkill(ActiveSkillSO skill)
    {
        if (!MySkills.ContainsKey(skill.ID))
        {
            MySkills[skill.ID] = skill;
        }
        else
        {
            //TODO : 재화 돌려줌
        }
    }
}