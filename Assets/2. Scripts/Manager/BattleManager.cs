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
    public List<int> PartyUnitsID;
    public List<int> EnemyUnitsID;
    public List<Unit> PartyUnits;
    public List<Unit> EnemyUnits;
    public TurnHandler TurnHandler { get; private set; }

    private List<Unit> allUnits = new List<Unit>();
    public event Action OnBattleEnd;


    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        if (PlayerDeckContainer.Instance.CurrentDeck.deckDatas.Count == 0)
            SetAlliesUnit(PartyUnitsID.Select(id => TableManager.Instance.GetTable<PlayerUnitTable>().GetDataByID(id)).ToList());
        else
        {
            SetAlliesUnit(PlayerDeckContainer.Instance.CurrentDeck);
        }

        if (PlayerDeckContainer.Instance.SelectedStage == null)
            SetEnemiesUnit(EnemyUnitsID.Select(id => TableManager.Instance.GetTable<MonsterTable>().GetDataByID(id)).ToList());
        else
        {
            SetEnemiesUnit(PlayerDeckContainer.Instance.SelectedStage.Monsters);
        }

        TurnHandler = new TurnHandler();
        SetAllUnits(PartyUnits.Concat(EnemyUnits).ToList());
    }

    public void SetAlliesUnit(List<PlayerUnitSO> units)
    {
        int index = 0;
        foreach (PlayerUnitSO playerUnitSo in units)
        {
            GameObject go = Instantiate(playerUnitSo.UnitPrefab, Vector3.zero, Quaternion.identity);
            go.transform.SetParent(PartyUnitsTrans[index].transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            Unit unit = go.GetComponent<Unit>();
            unit.Initialize(playerUnitSo);
            PartyUnits.Add(unit);
            index++;
        }
    }

    public void SetAlliesUnit(PlayerDeck playerDeck)
    {
        int index = 0;
        foreach (EntryDeckData deckData in playerDeck.deckDatas)
        {
            GameObject go = Instantiate(deckData.CharacterSo.UnitPrefab, Vector3.zero, Quaternion.identity);
            go.transform.SetParent(PartyUnitsTrans[index].transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            PlayerUnitController unit = go.GetComponent<PlayerUnitController>();
            unit.Initialize(deckData.CharacterSo);
            PartyUnits.Add(unit);
            foreach (EquipmentItem equipment in deckData.equippedItems.Values)
            {
                unit.EquipmentManager.EquipItem(equipment);
            }

            unit.SkillManager.selectedSkill = deckData.skillDatas.ToList();

            index++;
        }
    }

    public void SetEnemiesUnit(List<EnemyUnitSO> units)
    {
        int index = 0;
        foreach (EnemyUnitSO enemyUnitSo in units)
        {
            GameObject go = Instantiate(enemyUnitSo.UnitPrefab, Vector3.zero, Quaternion.identity);
            go.transform.SetParent(EnemyUnitsTrans[index].transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            EnemyUnitController unit = go.GetComponent<EnemyUnitController>();
            unit.Initialize(enemyUnitSo);
            unit.ChoiceAction();
            EnemyUnits.Add(unit);
            index++;
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
                continue;
            unit.StatusEffectManager?.OnTurnPassed();
        }


        Debug.Log("배틀 턴이 종료 되었습니다.");
        // allUnits.RemoveAll(u => u.IsDead);
        PartyUnits.ForEach(x => x.ChangeUnitState(PlayerUnitState.Idle));
        if (EnemyUnits.TrueForAll(x => x.IsDead))
        {
            Debug.Log("승리");
            PartyUnits.Where(x => !x.IsDead).ToList().ForEach(x => x.ChangeUnitState(PlayerUnitState.Victory));
        }

        foreach (var enemy in EnemyUnits)
        {
            EnemyUnitController enemyUnit = enemy as EnemyUnitController;
            if (enemyUnit != null && !enemyUnit.IsDead)
            {
                enemyUnit.ChoiceAction();
            }
        }
        CommandPlanner.Instance.Clear();    // 턴 종료되면 전략 플래너도 초기화
        InputManager.Instance.Initialize(); // 턴 종료되면 인풋매니저도 초기화
        OnBattleEnd?.Invoke();
    }

    public List<Unit> GetAllies(Unit unit)
    {
        if (PartyUnits.Contains(unit))
            return PartyUnits.Where(u => !u.IsDead && u != unit).ToList();
        else
            return EnemyUnits.Where(u => !u.IsDead && u != unit).ToList();
    }

    public List<Unit> GetEnemies(Unit unit)
    {
        if (PartyUnits.Contains(unit))
            return EnemyUnits.Where(u => !u.IsDead && u != unit).ToList();
        else
            return PartyUnits.Where(u => !u.IsDead && u != unit).ToList();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}