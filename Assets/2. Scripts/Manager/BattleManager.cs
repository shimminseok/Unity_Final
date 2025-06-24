using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : SceneOnlySingleton<BattleManager>
{
    public TurnHandler TurnHandler { get; private set; }


    private List<Unit> allUnits = new List<Unit>();

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        TurnHandler = new TurnHandler();
        //
        TurnHandler.Initialize(allUnits);
    }

    public void EndTurn()
    {
        foreach (Unit unit in allUnits)
        {
            if (unit.IsDead) continue;
            unit.StatusEffectManager?.OnTurnPassed();
        }

        allUnits.RemoveAll(u => u.IsDead);
        TurnHandler.Initialize(allUnits);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}