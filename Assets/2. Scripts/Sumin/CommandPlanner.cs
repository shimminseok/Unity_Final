using System.Collections.Generic;
using System.Linq;

// 현재 BattleManager와 연동되지 않은 상태
// Unit에서 행동을 직접 수행하고 있음

public class CommandPlanner : SceneOnlySingleton<CommandPlanner>
{
    // Unit과 Command Dictionary에 저장
    private Dictionary<Unit, IActionCommand> plannedCommands = new Dictionary<Unit, IActionCommand>();

    public void PlanAction(Unit unit, IActionCommand command)
    {
        plannedCommands[unit] = command;
    }

    public IActionCommand GetPlannedCommand(Unit unit)
    {
        plannedCommands.TryGetValue(unit, out var command);
        return command;
    }

    public bool HasPlannedCommand(Unit unit) => plannedCommands.ContainsKey(unit);

    public void Clear()
    {
        plannedCommands.Clear();
    }
}
