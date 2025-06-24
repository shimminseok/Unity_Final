using System.Collections.Generic;
using System.Linq;

public class TurnHandler
{
    private Queue<Unit> turnQueue = new Queue<Unit>();

    private Unit currentTurnUnit;

    private List<Unit> unitList;

    public void Initialize(List<Unit> units)
    {
        unitList = units.Where(u => !u.IsDead)
            .OrderByDescending(u => u.StatManager.GetValue(StatType.Speed))
            .ToList();

        turnQueue = new Queue<Unit>(unitList);
    }

    public void StartNextTurn()
    {
        if (turnQueue.Count == 0)
        {
            BattleManager.Instance.EndTurn();
            return;
        }

        currentTurnUnit = turnQueue.Dequeue();
        currentTurnUnit.StartTurn();
    }

    public void OnUnitTurnEnd()
    {
        currentTurnUnit.EndTurn();

        if (turnQueue.Count > 0)
        {
            StartNextTurn();
        }
        else
        {
            // 전체 라운드 종료
            BattleManager.Instance.EndTurn();
        }
    }

    public void RefillTurnQueue()
    {
        unitList.RemoveAll(u => u.IsDead);
        turnQueue = new Queue<Unit>(unitList);
    }
}