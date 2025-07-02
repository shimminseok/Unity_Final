using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : SceneOnlySingleton<InputManager>
{
    [SerializeField] private Camera mainCam;

    [Header("선택 타겟 레이어 설정")]
    [SerializeField] private LayerMask unitLayer;
    [SerializeField] private LayerMask playerUnitLayer;
    [SerializeField] private LayerMask enemyUnitLayer;

    private InputStateMachine inputStateMachine;
    private InputContext context;
    private UnitSelector selector;

    private void Start()
    {
        if (mainCam == null)
        {
            mainCam = Camera.main;
        }

        context = new InputContext
        {
            UnitLayer = unitLayer,
            PlayerUnitLayer = playerUnitLayer,
            EnemyUnitLayer = enemyUnitLayer,
            OpenSkillUI = unit => UIManager.Instance.GetUIComponent<BattleSceneSkillUI>().UpdateSkillList(unit),
            CloseSkillUI = () => UIManager.Instance.Close<BattleSceneSkillUI>(),
            CloseStartButtonUI = () => UIManager.Instance.Close<BattleSceneStartButton>(),
            OpenStartButtonUI = () => UIManager.Instance.Open<BattleSceneStartButton>(),
            PlanAttackCommand = (executor, target) =>
            {
                IActionCommand command = new AttackCommand(executor, target);
                CommandPlanner.Instance.PlanAction(executor, command);
            }
        };

        inputStateMachine = new InputStateMachine();
        selector = new UnitSelector(context, mainCam);

        inputStateMachine.CreateInputState<SelectExecuterState>(new SelectExecuterState(context, selector, inputStateMachine));
        inputStateMachine.CreateInputState<SelectSkillState>(new SelectSkillState(context, inputStateMachine));
        inputStateMachine.CreateInputState<SelectTargetState>(new SelectTargetState(context, selector, inputStateMachine));
        inputStateMachine.CreateInputState<InputDisabledState>(new InputDisabledState(context, selector));

        StartCoroutine(WaitForBattleManagerInit());
    }

    // BattleManager가 초기화 완료된 후 InputManager의 SelectExecuter 상태 시작 (호출 순서 문제 해결)
    private IEnumerator WaitForBattleManagerInit()
    {
        yield return new WaitUntil(() => BattleManager.Instance != null && BattleManager.Instance.PartyUnits.Count > 0);

        inputStateMachine.ChangeState<SelectExecuterState>();
    }

    void Update()
    {
        inputStateMachine.Update();
    }

    // Input매니저 초기화
    public void Initialize()
    {
        inputStateMachine.ChangeState<SelectExecuterState>();
    }

    // Skill 선택 중 나가기 버튼
    public void OnClickSkillExitButton()
    {
        inputStateMachine.ChangeState<SelectExecuterState>();
    }

    // Start 버튼
    public void OnClickTurnStartButton()
    {
        UIManager.Instance.Close<BattleSceneStartButton>();
        BattleManager.Instance.StartTurn();
        inputStateMachine.ChangeState<InputDisabledState>();
    }

    // 스킬 선택 버튼
    public void SelectSkill(int index)
    {
        // 스킬 인덱스 받아서 교체
        if (context.SelectedExcuter is PlayerUnitController playerUnit)
        {
            playerUnit.SkillController.ChangeSkill(index);
            context.SelectedSkill = playerUnit.SkillController.CurrentSkillData;

            // 타겟 인디케이터 업데이트
            selector.ShowSelectableUnits(unitLayer, false);
            context.TargetLayer = selector.GetLayerFromSkill(context.SelectedSkill);
            selector.ShowSelectableUnits(context.TargetLayer, true);
        }
        ChangeSelectedUnitAction(ActionType.SKill);
    }

    // 기본 공격 선택 버튼
    public void SelectBasicAttack()
    {
        ChangeSelectedUnitAction(ActionType.Attack);

        // 타겟 인디케이터 업데이트
        selector.ShowSelectableUnits(unitLayer, false);
        selector.ShowSelectableUnits(enemyUnitLayer, true);
    }

    // 선택한 액션 타입(기본공격/스킬)에 따라 ChangeAction하는 함수
    private void ChangeSelectedUnitAction(ActionType actionType)
    {
        if (context.SelectedExcuter is PlayerUnitController playerUnit)
        {
            playerUnit.ChangeAction(actionType);
        }
    }
}
