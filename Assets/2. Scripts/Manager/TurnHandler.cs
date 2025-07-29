using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnHandler
{
    private Queue<Unit> turnQueue = new();

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
        currentTurnUnit = turnQueue.Dequeue();
        CameraManager.Instance.ChangeFollowTarget(currentTurnUnit);
        currentTurnUnit.StartTurn();
    }

    public void OnUnitTurnEnd()
    {
        if (turnQueue.Count > 0)
        {
            StartNextTurn();
        }
        else
        {
            // 전체 라운드 종료
            BattleManager.Instance.EndTurn();

            // 튜토리얼에서 TurnChanged 이벤트 발행
            TutorialManager tutorial = TutorialManager.Instance;

            if (tutorial != null && tutorial.IsActive &&
                tutorial.CurrentStep?.ActionData is TriggerWaitActionData triggerData &&
                triggerData.ActionType == TutorialActionType.TriggerWait &&
                triggerData.triggerEventName == "TurnChanged")
            {
                EventBus.Publish("TurnChanged");
            }

        }
    }

    public void RefillTurnQueue()
    {
        unitList.RemoveAll(u => u.IsDead);
        unitList = unitList.OrderByDescending(u => u.StatManager.GetValue(StatType.Speed))
            .ToList();
        turnQueue = new Queue<Unit>(unitList);
    }
}