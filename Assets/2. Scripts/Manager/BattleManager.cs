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
    public TurnHandler    TurnHandler    { get; private set; }
    public CommandPlanner CommandPlanner { get; private set; } // 전략 페이즈의 플래너 가저오기


    private List<Unit> allUnits = new List<Unit>();
    public event Action OnBattleEnd;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        SetAlliesUnit(PartyUnitsID.Select(id => TableManager.Instance.GetTable<PlayerUnitTable>().GetDataByID(id)).ToList());
        SetEnemiesUnit(EnemyUnitsID.Select(id => TableManager.Instance.GetTable<MonsterTable>().GetDataByID(id)).ToList());

        TurnHandler = new TurnHandler();
        SetAllUnits(PartyUnits.Concat(EnemyUnits).ToList());
        UIManager.Instance.Open<BattleSceneStartButton>(); // 배틀씬 시작되면 켜져야함.
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

    public void SetEnemiesUnit(List<EnemyUnitSO> units)
    {
        int index = 0;
        foreach (EnemyUnitSO enemyUnitSo in units)
        {
            GameObject go = Instantiate(enemyUnitSo.UnitPrefab, Vector3.zero, Quaternion.identity);
            go.transform.SetParent(EnemyUnitsTrans[index].transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            Unit unit = go.GetComponent<Unit>();
            unit.Initialize(enemyUnitSo);
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
        UIManager.Instance.Close<BattleSceneStartButton>(); // 턴 시작되면 start 버튼 끄기
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
        allUnits.RemoveAll(u => u.IsDead);
        TurnHandler.RefillTurnQueue();
        CommandPlanner?.Clear(); // 턴 종료되면 전략 플래너도 초기화
        OnBattleEnd?.Invoke();
        UIManager.Instance.Open<BattleSceneStartButton>(); // 턴 종료되면 start 버튼 켜기
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