using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : SceneOnlySingleton<BattleManager>
{
    private TurnHandler turnHandler;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        turnHandler = new TurnHandler();
        //
        turnHandler.Initialize(null);
    }

    private void Update()
    {
    }

    public void EndTurn()
    {
        turnHandler.Initialize(null);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}