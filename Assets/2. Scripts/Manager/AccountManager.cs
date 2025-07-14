using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AccountManager : Singleton<AccountManager>
{
    public int Gold      { get; private set; } = 0;
    public int Opal      { get; private set; } = 3000;
    public int BestStage { get; private set; } = 1010109;

    public Dictionary<int, EntryDeckData> MyPlayerUnits = new Dictionary<int, EntryDeckData>();
    public Dictionary<int, ActiveSkillSO> MySkills = new Dictionary<int, ActiveSkillSO>();
    public event Action<int> OnGoldChanged;
    public event Action<int> OnOpalChanged;


    private List<int> orderedStageIds;

    protected override void Awake()
    {
        Application.targetFrameRate = 60;
        base.Awake();
        foreach (PlayerUnitSO playerUnitSo in TableManager.Instance.GetTable<PlayerUnitTable>().DataDic.Values)
        {
            AddPlayerUnit(playerUnitSo);
        }

        orderedStageIds = TableManager.Instance.GetTable<StageTable>().DataDic.Keys.OrderBy(id => id).ToList();
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

    public void UpdateBestStage(StageSO stage)
    {
        if (BestStage < stage.ID)
        {
            BestStage = stage.ID;
            RewardManager.Instance.GiveReward(stage.FirstClearReward.Id);
        }
    }

    public void SetBestStage(int stage)
    {
        BestStage = stage;
    }

    public int GetNextStageId(int currentStageId)
    {
        int chapterId = currentStageId / 10000; // 예: 101
        int stageId   = currentStageId % 10000; // 예: 0101 ~ 0110
        int stageNum  = stageId % 100;          // 예: 01 ~ 10

        const int maxStagePerChapter = 10;

        if (stageNum < maxStagePerChapter)
        {
            // 같은 챕터에서 다음 스테이지로
            return currentStageId + 1;
        }
        else
        {
            // 다음 챕터의 첫 스테이지로 (챕터+1, 스테이지 0101)
            int nextChapterId = chapterId + 1;
            return nextChapterId * 10000 + 101;
        }
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