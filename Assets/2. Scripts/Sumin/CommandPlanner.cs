using System.Collections.Generic;
using System.Linq;

public class CommandPlanner : SceneOnlySingleton<CommandPlanner>
{
    // Unit과 Command Dictionary에 저장
    private Dictionary<Unit, IActionCommand> plannedCommands = new Dictionary<Unit, IActionCommand>();

    public void PlanAction(Unit unit, IActionCommand command)
    {
        plannedCommands[unit] = command;
    }

    public List<IActionCommand> GetPlannedCommands()
    {
        return plannedCommands.Values.ToList();
    }

    public void Clear()
    {
        plannedCommands.Clear();
    }
}
