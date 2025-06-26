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


    private List<Unit> allUnits = new List<Unit>();

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        TurnHandler = new TurnHandler();

        SetAllUnits(PartyUnits.Concat(EnumyUnits).ToList());
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
            if (!unit.IsDead)
            {
                unit.StatusEffectManager?.OnTurnPassed();
            }
        }


        if (EnumyUnits.TrueForAll(x => x.IsDead))
        {
            Debug.Log("게임 승리");
        }
        else if (PartyUnits.TrueForAll(x => x.IsDead))
        {
            Debug.Log("게임 패배");
        }

        TurnHandler.RefillTurnQueue();
        Debug.Log("라운드 종료");
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