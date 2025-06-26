using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : SceneOnlySingleton<BattleManager>
{
    //Test
    public List<Unit> PartyUnits;
    public List<Unit> EnumyUnits;
    public TurnHandler TurnHandler { get; private set; }
    public ActionPlanner ActionPlanner { get; private set; } // 전략 페이즈의 플래너 가저오기


    private List<Unit> allUnits = new List<Unit>();

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        TurnHandler = new TurnHandler();

        allUnits = PartyUnits.Concat(EnumyUnits).ToList();
        //
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

        allUnits.RemoveAll(u => u.IsDead);
        TurnHandler.RefillTurnQueue();
        ActionPlanner.Clear();  // 턴 종료되면 전략 플래너도 초기화
    }

    public List<Unit> GetAllies(Unit unit)
    {
        if (PartyUnits.Contains(unit))
            return PartyUnits.Where(u => !u.IsDead && u != unit).ToList();
        else
            return EnumyUnits.Where(u => !u.IsDead && u != unit).ToList();
    }

    public List<Unit> GetEnemies(Unit unit)
    {
        if (PartyUnits.Contains(unit))
            return EnumyUnits.Where(u => !u.IsDead && u != unit).ToList();
        else
            return PartyUnits.Where(u => !u.IsDead && u != unit).ToList();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}