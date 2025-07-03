using System.Collections.Generic;
using UnityEngine;

// 유닛 선택하는 기능을 하는 클래스
public class UnitSelector
{
    private readonly Camera cam;
    private readonly InputContext context;

    // InputManager에서 생성자를 통해 카메라 연결
    public UnitSelector(InputContext context, Camera cam)
    {
        this.cam = cam;
        this.context = context;
    }

    // 유닛 선택 메서드
    public bool TrySelectUnit(LayerMask selectableUnit, out ISelectable selected)
    {
        // selected에 선택한 유닛 넣어주고, true로 반환
        selected = null;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, selectableUnit))
        {
            selected = hit.transform.GetComponent<ISelectable>();
            if (Input.GetMouseButtonDown(0)) return selected != null;
        }
        return false;
    }

    // 선택 가능한 유닛 레이어에 Selectable Indicator 띄워주기
    public void ShowSelectableUnits(LayerMask layer, bool show)
    {
        List<Unit> selectableUnits = new List<Unit>();

        selectableUnits.AddRange(GetUnitsFromLayer(layer));

        foreach (Unit unit in selectableUnits)
        {
            unit.ToggleSelectableIndicator(show);
        }
    }

    // 각 레이어에 맞는 유닛들을 배틀매니저에서 받아오기
    public List<Unit> GetUnitsFromLayer(LayerMask layer)
    {
        List<Unit> units = new();
        if ((layer.value & context.PlayerUnitLayer) != 0)
            units.AddRange(BattleManager.Instance.PartyUnits);
        if ((layer.value & context.EnemyUnitLayer) != 0)
            units.AddRange(BattleManager.Instance.EnemyUnits);
        return units;
    }

    // 선택한 스킬의 타겟 진영 받아오기
    public LayerMask GetLayerFromSkill(SkillData skill)
    {
        // 기본공격이면 skill이 null 이므로 Enemy, 스킬은 SelectCampType에 따라.
        return skill == null ? context.EnemyUnitLayer : skill.skillSo.selectCamp switch
        {
            SelectCampType.Enemy => context.EnemyUnitLayer,
            SelectCampType.Player => context.PlayerUnitLayer,
            SelectCampType.BothSide => context.UnitLayer,
            _ => context.UnitLayer
        };
    }

    // 하이라이트 초기화
    public void InitializeHighlight()
    {
        Unit executer = context.SelectedExecuter.SelectedUnit;

        if (executer is PlayerUnitController playerUnit)
        {
            int skillCount = executer.SkillController.skills.Count;
            for (int i = 0; i < skillCount; i++)
            {
                context.HighlightSkillSlotUI?.Invoke(false, i);
            }
        }
        context.HighlightBasicAttackUI?.Invoke(false);

        var command = CommandPlanner.Instance.GetPlannedCommand(executer);
        if (CommandPlanner.Instance.HasPlannedCommand(executer))
            command.Target?.ToggleSelectedIndicator(false);
    }

    public void ShowPrevCommand(Unit unit)
    {
        InitializeHighlight();
        var command = CommandPlanner.Instance.GetPlannedCommand(unit);
        if (CommandPlanner.Instance.HasPlannedCommand(unit))
        {
            if (command.SkillData != null)
            {
                int index = unit.SkillController.GetSkillIndex(command.SkillData);
                context.HighlightSkillSlotUI?.Invoke(true, index);
            }
            else
            {
                context.HighlightBasicAttackUI?.Invoke(true);
            }
            command.Target?.ToggleSelectedIndicator(true);
        }
    }
}
