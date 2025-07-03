// 인풋에서 관여하는 데이터, Delegate
using System.Collections.Generic;
using System;
using UnityEngine;

public class InputContext
{
    public ISelectable SelectedExcuter;
    public SkillData SelectedSkill;
    public LayerMask TargetLayer;
    public ISelectable SelectedTarget;

    public LayerMask UnitLayer;
    public LayerMask PlayerUnitLayer;
    public LayerMask EnemyUnitLayer;

    public Action<Unit> OpenSkillUI;
    public Action CloseSkillUI;
    public Action CloseStartButtonUI;
    public Action OpenStartButtonUI;
    public Action<Unit, Unit, SkillData> PlanActionCommand;
    public Action<int> HighlightSkillSlotUI;
    public Action HighlightBasicAttackUI;
}
