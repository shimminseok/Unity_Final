using System.Collections.Generic;
using UnityEngine;

// Unit에서 행동을 직접 수행하고 있음

public class CommandPlanner : SceneOnlySingleton<CommandPlanner>
{
    // Unit과 Command Dictionary에 저장
    private Dictionary<Unit, IActionCommand> plannedCommands = new Dictionary<Unit, IActionCommand>();
    
    // 실행할 유닛과 커맨드 액션 plannedCommands에 저장
    public void PlanAction(IActionCommand command)
    {
        if (command == null || command.Executer == null)
            return;

        plannedCommands[command.Executer] = command;
    }

    // 턴 시작될 때 저장된 커맨드들을 실행시킨다.
    public void ExecutePlannedActions()
    {
        foreach (var command in plannedCommands.Values)
        {
            command.Execute();
        }
        Clear();
    }

    // 저장된 커맨드 받아오기
    public IActionCommand GetPlannedCommand(Unit unit)
    {
        plannedCommands.TryGetValue(unit, out var command);
        return command;
    }

    // 저장된 커맨드가 있는지 확인
    public bool HasPlannedCommand(Unit unit) => plannedCommands.ContainsKey(unit);

    // 저장된 커맨드 초기화
    public void Clear()
    {
        plannedCommands.Clear();
    }
}
