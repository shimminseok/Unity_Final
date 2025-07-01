using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEngine.UI.CanvasScaler;

// 추후 StateMachine으로 리팩토링하면 좋다.

// 나중에 Enum으로 이동
public enum InputPhase
{
    SelectExecuter, // 커맨드를 수행할 플레이어 유닛 선택 상태
    SelectSkill,    // 유닛이 사용할 스킬 혹은 기본 공격 선택 상태
    SelectTarget    // 유닛이 스킬을 사용할 타겟 선택 상태
}

// 전투 씬에서 플레이어 선택 Input 관리
public class InputManager : SceneOnlySingleton<InputManager>
{
    [SerializeField] private Camera mainCam;
    [SerializeField] private BattleSceneSkillUI skillUI;

    [Header("선택 타겟 레이어 설정")]
    [SerializeField] private LayerMask unitLayer;

    [SerializeField] private LayerMask playerUnitLayer;
    [SerializeField] private LayerMask enemyUnitLayer;

    private LayerMask targetLayer;
    private ISelectable selectedExecuterUnit;
    private ISelectable selectedTargetUnit;
    private ISelectable selectable;
    private InputPhase currentPhase = InputPhase.SelectExecuter;
    public SkillData SelectedSkillData { get; set; }

    void Start()
    {
        if (mainCam == null)
        {
            mainCam = Camera.main;
        }
    }

    void Update()
    {
        switch (currentPhase)
        {
            case InputPhase.SelectExecuter:
                OnClickPlayerUnit();
                break;
            case InputPhase.SelectSkill:
                // 버튼 동작시까지 대기
                break;
            case InputPhase.SelectTarget:
                OnClickTargetUnit();
                break;
        }
    }

    // 플레이어 유닛 선택
    private void OnClickPlayerUnit()
    {
        ShowSelectableUnit(playerUnitLayer, true);
        Ray        ray = mainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, playerUnitLayer))
        {
            selectable = hit.transform.GetComponent<ISelectable>();

            if (Input.GetMouseButtonDown(0))
            {
                SelectUnit(selectable);

                // 유닛 선택하면 스킬 선택 페이즈로 전환
                currentPhase = InputPhase.SelectSkill;
                Debug.Log($"플레이어 유닛 선택 : {selectedExecuterUnit}");

                // 스킬 슬롯 UI에 유닛이 가지고 있는 스킬 데이터 연동
                skillUI.UpdateSkillList(selectedExecuterUnit.SelectedUnit);
                ShowSelectableUnit(playerUnitLayer, false); // 선택 가능 인디케이터 끄기
                UIManager.Instance.Close<BattleSceneStartButton>(); // 플레이어 유닛 선택 시작하면 start 버튼 끄기
            }
        }
        else
        {
            return;
        }
    }

    // 유닛이 사용할 스킬 선택
    public void SelectSkill(int index)
    {
        if (selectedExecuterUnit == null)
        {
            Debug.Log("플레이어 유닛 선택하지 않음!");
        }

        // 스킬 인덱스 받아서 교체
        if (selectedExecuterUnit is PlayerUnitController playerUnit)
        {
            playerUnit.SkillController.ChangeSkill(index);
        }

        ChangeSelectedUnitAction(ActionType.SKill);
        Debug.Log($"스킬 {index}번 선택");
    }

    // 플레이어 유닛이 기본공격 수행
    public void SelectBasicAttack()
    {
        ChangeSelectedUnitAction(ActionType.Attack);
        Debug.Log("기본 공격 선택");
    }

    // 선택한 액션 타입(기본공격/스킬)에 따라 ChangeAction하는 함수
    private void ChangeSelectedUnitAction(ActionType actionType)
    {
        currentPhase = InputPhase.SelectTarget;
        if (selectedExecuterUnit is PlayerUnitController playerUnit)
        {
            playerUnit.ChangeAction(actionType);
        }
    }

    // 공격 혹은 스킬 사용할 타겟 유닛 선택
    private void OnClickTargetUnit()
    {
        // 만약 스킬이라면? 스킬 타겟에 따라 레이어를 변경해줘야함
        if (SelectedSkillData != null)
        {
            targetLayer = GetTargetLayerMask(SelectedSkillData.selectedCamp);
        }
        else
        {
            targetLayer = enemyUnitLayer;
        }

        ShowSelectableUnit(targetLayer, true); // 선택 가능한 유닛 인디케이터 켜기

        Ray        ray = mainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, targetLayer))
        {
            selectedTargetUnit = hit.transform.GetComponent<ISelectable>();

            if (Input.GetMouseButtonDown(0))
            {
                Unit targetUnit = selectedTargetUnit.SelectedUnit;
                Unit executer   = selectedExecuterUnit.SelectedUnit;

                targetUnit.PlaySelectEffect(); // 선택했을때 이펙트 띄워주기

                // playerUnit에게 선택한 mainTarget 전달하기
                if (selectedExecuterUnit is PlayerUnitController playerUnit)
                {
                    playerUnit.SkillController.mainTarget = targetUnit;
                }

                executer.SetTarget(targetUnit);
                Debug.Log($"타겟 유닛 선택 : {targetUnit}");

                ShowSelectableUnit(targetLayer, false); // 선택 완료하면 인디케이터 꺼주기

                // 커맨드 생성
                IActionCommand command = new AttackCommand(executer, targetUnit);
                CommandPlanner.Instance.PlanAction(executer, command);

                // 다음 선택
                DeselectUnit();
                currentPhase = InputPhase.SelectExecuter;
                UIManager.Instance.Close<BattleSceneSkillUI>();

                // 타겟까지 설정되면 Start 버튼 활성화
                UIManager.Instance.Open<BattleSceneStartButton>();
            }
        }
        else
        {
            return;
        }
    }

    public void OnClickTurnStartButton()
    {
        BattleManager.Instance.StartTurn();
        ShowSelectableUnit(unitLayer, false);
    }

    // 선택한 스킬의 타겟 진영 받아오기
    private LayerMask GetTargetLayerMask(SelectCampType selectCamp)
    {
        switch (selectCamp)
        {
            case SelectCampType.Enemy:
                return enemyUnitLayer;
            case SelectCampType.Player:
                return playerUnitLayer;
            case SelectCampType.BothSide:
                return unitLayer;
            default:
                return unitLayer;
        }
    }

    private void DeselectUnit()
    {
        if (selectedExecuterUnit != null)
            selectedExecuterUnit.ToggleSelectedIndicator(false);

        selectedExecuterUnit = null;
    }

    private void SelectUnit(ISelectable unit)
    {
        selectedExecuterUnit = unit;
        unit.PlaySelectEffect();
        unit.ToggleSelectedIndicator(true);
    }

    // 선택 가능한 유닛 레이어에 Selectable Indicator 띄워주기
    private void ShowSelectableUnit(LayerMask layer, bool isSelectable)
    {
        List<Unit> targetUnits = new List<Unit>();

        if ((layer.value & playerUnitLayer.value) != 0)
        {
            targetUnits.AddRange(BattleManager.Instance.PartyUnits);
        }

        if ((layer.value & enemyUnitLayer.value) != 0)
        {
            targetUnits.AddRange(BattleManager.Instance.EnemyUnits);
        }

        foreach (Unit unit in targetUnits)
        {
            unit.ToggleSelectableIndicator(isSelectable);
        }
    }
}