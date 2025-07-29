using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : SceneOnlySingleton<BattleManager>
{
    //Test
    public List<Transform> PartyUnitsTrans;
    public List<Transform> EnemyUnitsTrans;
    public List<Unit> PartyUnits;
    public List<Unit> EnemyUnits;
    public TurnHandler TurnHandler { get; private set; }

    private StageSO currentStage;
    private List<Unit> allUnits = new();
    public event Action OnBattleEnd;

    private UIReward uiReward;

    public int TurnCount = 1;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        currentStage = PlayerDeckContainer.Instance.SelectedStage;

        SetAlliesUnit(PlayerDeckContainer.Instance.CurrentDeck);

        SetEnemiesUnit(currentStage.Monsters);

        TurnHandler = new TurnHandler();
        SetAllUnits(PartyUnits.Concat(EnemyUnits).ToList());
    }

    private void SetAlliesUnit(PlayerDeck playerDeck)
    {
        for (int i = 0; i < playerDeck.DeckDatas.Count; i++)
        {
            if (playerDeck.DeckDatas[i] == null)
            {
                continue;
            }

            EntryDeckData deckData = playerDeck.DeckDatas[i];
            GameObject    go       = Instantiate(deckData.CharacterSo.UnitPrefab, PartyUnitsTrans[i], false);
            Unit          unit     = go.GetComponent<Unit>();
            unit.Initialize(new UnitSpawnData { UnitSo = deckData.CharacterSo, DeckData = deckData });

            PartyUnits.Add(unit as PlayerUnitController);
        }
    }

    public void SetEnemiesUnit(List<EnemyUnitSO> units)
    {
        for (int i = 0; i < units.Count; i++)
        {
            EnemyUnitSO unitSo = units[i];
            GameObject  go     = Instantiate(unitSo.UnitPrefab, EnemyUnitsTrans[i], false);
            Unit        unit   = go.GetComponent<Unit>();
            unit.Initialize(new UnitSpawnData
            {
                UnitSo = unitSo, DeckData = null // Enemy
            });
            EnemyUnitController eu = unit as EnemyUnitController;
            if (eu != null)
            {
                eu.ChoiceAction();
                EnemyUnits.Add(eu);
            }
        }
    }

    public void SetAllUnits(List<Unit> units)
    {
        allUnits = units;
        TurnHandler.Initialize(allUnits);
    }

    public void StartTurn()
    {
        TurnHandler.StartNextTurn();
    }

    public void EndTurn()
    {
        foreach (Unit unit in allUnits)
        {
            if (unit.IsDead)
            {
                continue;
            }

            unit.CurrentEmotion.AddStack(unit);
            unit.StatusEffectManager?.OnTurnPassed();
        }


        Debug.Log("배틀 턴이 종료 되었습니다.");
        PartyUnits.ForEach(x => x.ChangeUnitState(PlayerUnitState.Idle));
        if (EnemyUnits.TrueForAll(x => x.IsDead))
        {
            OnStageClear();
            return;
        }

        if (PartyUnits.TrueForAll(x => x.IsDead))
        {
            OnStageFail();
            return;
        }

        TurnHandler.RefillTurnQueue();
        foreach (EnemyUnitController enemy in EnemyUnits)
        {
            enemy.ChoiceAction();
        }

        CommandPlanner.Instance.Clear();    // 턴 종료되면 전략 플래너도 초기화
        InputManager.Instance.Initialize(); // 턴 종료되면 인풋매니저도 초기화
        TurnCount++;                        // 턴 종료되면 턴 수 +1
        OnBattleEnd?.Invoke();
    }

    public List<Unit> GetAllies(Unit unit)
    {
        if (PartyUnits.Contains(unit))
        {
            return PartyUnits.Where(u => !u.IsDead && u != unit).ToList();
        }
        else
        {
            return EnemyUnits.Where(u => !u.IsDead && u != unit).ToList();
        }
    }

    public List<Unit> GetEnemies(Unit unit)
    {
        if (PartyUnits.Contains(unit))
        {
            return EnemyUnits.Where(u => !u.IsDead && u != unit).ToList();
        }
        else
        {
            return PartyUnits.Where(u => !u.IsDead && u != unit).ToList();
        }
    }

    private void OnStageClear()
    {
        // 튜토리얼에서 BattleVictory 이벤트 발행
        TutorialManager tutorial = TutorialManager.Instance;

        if (tutorial != null && tutorial.IsActive &&
            TutorialManager.Instance.CurrentStep.ActionData.ActionType == TutorialActionType.TriggerWait &&
            TutorialManager.Instance.CurrentStep.ActionData is TriggerWaitActionData triggerData &&
            triggerData.triggerEventName == "BattleVictory")
        {
            EventBus.Publish("BattleVictory");
        }

        PartyUnits.Where(x => !x.IsDead).ToList().ForEach(x => x.ChangeUnitState(PlayerUnitState.Victory));
        string rewardKey = $"{currentStage.ID}_Clear_Reward";
        RewardManager.Instance.AddReward(rewardKey);
        AccountManager.Instance.UpdateBestStage(currentStage);
        RewardManager.Instance.GiveRewardAndOpenUI(() => LoadSceneManager.Instance.LoadScene("DeckBuildingScene"));
    }

    private void OnStageFail()
    {
        UIManager.Instance.Open(PopupManager.Instance.GetUIComponent<UIDefeat>());
        return;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}

public class UnitSpawnData
{
    public UnitSO UnitSo;
    public EntryDeckData DeckData; // null이면 Enemy
}