using System;
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
            PlanActionCommand = (executor, target, skillData) =>
            {
                IActionCommand command = new ActionCommand(executor, target, skillData);
                CommandPlanner.Instance.PlanAction(command);
                Debug.Log($"커맨드 등록 : {executor}, {target}, {(skillData == null ? "기본공격" : skillData.skillSo.name)}");

            },
            HighlightSkillSlotUI = (toggle, index) =>
            {
                UIManager.Instance.GetUIComponent<BattleSceneSkillUI>().ToggleHighlightSkillSlot(toggle, index);
            },
            HighlightBasicAttackUI = (toggle) =>
            {
                UIManager.Instance.GetUIComponent<BattleSceneSkillUI>().ToggleHighlightBasicAttack(toggle);
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

    public void DebugMethod()
    {
        Debug.Log("유저 선택 진입");
    }

    // Input매니저 초기화
    public void Initialize()
    {
        inputStateMachine.ChangeState<SelectExecuterState>();
        selector.InitializeHighlight();
    }

    // Skill 선택 중 나가기 버튼
    public void OnClickSkillExitButton()
    {
        // 인디케이터 꺼주기
        selector.InitializeHighlight();
        selector.ShowSelectableUnits(context.UnitLayer, false);
        
        // Start 버튼 활성화
        context.OpenStartButtonUI?.Invoke();
        
        inputStateMachine.ChangeState<SelectExecuterState>();
    }

    // Start 버튼
    public void OnClickTurnStartButton()
    {
        CommandPlanner.Instance.ExecutePlannedActions();
        // start 버튼 비활성화
        context.CloseStartButtonUI?.Invoke();

        // 배틀매니저 턴 시작
        BattleManager.Instance.StartTurn();
        
        // 인풋 불가 상태로 진입
        inputStateMachine.ChangeState<InputDisabledState>();
    }

    // 스킬 선택 버튼
    public void SelectSkill(int index)
    {
        // 스킬 인덱스 받아서 context에 저장
        if (context.SelectedExecuter is PlayerUnitController playerUnit)
        {
            //playerUnit.SkillController.ChangeSkill(index);
            context.SelectedSkill = playerUnit.SkillController.GetSkillData(index);

            UpdateTargetIndicator();
        }
        //ChangeSelectedUnitAction(ActionType.SKill);
    }

    // 기본 공격 선택 버튼
    public void SelectBasicAttack()
    {
        context.SelectedSkill = null;
        //ChangeSelectedUnitAction(ActionType.Attack);

        inputStateMachine.ChangeState<SelectTargetState>();

        UpdateTargetIndicator();
    }

    // 선택한 액션 타입(기본공격/스킬)에 따라 ChangeAction하는 함수
    private void ChangeSelectedUnitAction(ActionType actionType)
    {
        if (context.SelectedExecuter is PlayerUnitController playerUnit)
        {
            playerUnit.ChangeAction(actionType);
        }
    }

    // 타겟 인디케이터 업데이트
    private void UpdateTargetIndicator()
    {
        selector.ShowSelectableUnits(unitLayer, false);
        context.TargetLayer = selector.GetLayerFromSkill(context.SelectedSkill);
        selector.ShowSelectableUnits(context.TargetLayer, true);
    }
}
