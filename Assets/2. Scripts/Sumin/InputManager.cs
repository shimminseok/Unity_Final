using Unity.VisualScripting;
using UnityEngine;
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
    private ISelectable selectedPlayerUnit;
    private ISelectable targetUnit;
    private InputPhase currentPhase = InputPhase.SelectExecuter;
    public Skill SelectedSkill { get; set; }

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
        if (Input.GetMouseButtonDown(0))
        {
            Ray        ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, playerUnitLayer))
            {
                ISelectable selectable = hit.transform.GetComponent<ISelectable>();
                SelectUnit(selectable);

                // 유닛 선택하면 스킬 선택 페이즈로 전환
                currentPhase = InputPhase.SelectSkill;
                Debug.Log($"플레이어 유닛 선택 : {selectedPlayerUnit}");

                // 스킬 슬롯 UI에 유닛이 가지고 있는 스킬 데이터 연동
                skillUI.UpdateSkillList(selectable.SelectedUnit);
            }
            else
            {
                return;
            }
        }
    }

    // 유닛이 사용할 스킬 선택
    public void SelectSkill(int index)
    {
        if (selectedPlayerUnit == null)
        {
            Debug.Log("플레이어 유닛 선택하지 않음!");
        }

        // 스킬 인덱스 받아서 교체
        if (selectedPlayerUnit is PlayerUnitController playerUnit)
        {
            playerUnit.PlayerSkillController.ChangeSkill(index);
        }
        ChangeSelectedUnitAction(PlayerActionType.SKill);
        Debug.Log($"스킬 {index}번 선택");
    }

    // 플레이어 유닛이 기본공격 수행
    public void SelectBasicAttack()
    {
        ChangeSelectedUnitAction(PlayerActionType.Attack);
        Debug.Log("기본 공격 선택");
    }

    // 선택한 액션 타입(기본공격/스킬)에 따라 ChangeAction하는 함수
    private void ChangeSelectedUnitAction(PlayerActionType actionType)
    {
        currentPhase = InputPhase.SelectTarget;
        if (selectedPlayerUnit is PlayerUnitController playerUnit)
        {
            playerUnit.ChangeAction(actionType);
        }
    }

    // 공격 혹은 스킬 사용할 타겟 유닛 선택
    private void OnClickTargetUnit()
    {
        // 만약 스킬이라면? 스킬 타겟에 따라 레이어를 변경해줘야함
        if (SelectedSkill != null)
        {
            targetLayer = GetTargetLayerMask(SelectedSkill.selectedCamp);
        }
        else
        {
            targetLayer = enemyUnitLayer;
        }

        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, targetLayer))
        {
            ISelectable targetSelectable = hit.transform.GetComponent<ISelectable>();
            targetSelectable.OnSelect();

            if (Input.GetMouseButtonDown(0))
            {
                Unit targetUnit = targetSelectable.SelectedUnit;
                Unit executer = selectedPlayerUnit.SelectedUnit;

                // playerUnit에게 선택한 mainTarget 전달하기
                if (selectedPlayerUnit is PlayerUnitController playerUnit)
                {
                    playerUnit.PlayerSkillController.mainTarget = targetUnit;
                }

                executer.SetTarget(targetUnit);
                Debug.Log($"타겟 유닛 선택 : {targetUnit}");

                // 커맨드 생성
                IActionCommand command = new AttackCommand(executer, targetUnit);
                CommandPlanner.Instance.PlanAction(executer, command);
                Debug.Log($"수행 : {executer} | 타겟 : {targetUnit}");


                // 다음 선택
                DeselectUnit();
                currentPhase = InputPhase.SelectExecuter;
            }
            
        }
        else
        {
            return;
        }

        
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
        if (selectedPlayerUnit != null)
            selectedPlayerUnit.OnDeselect();

        selectedPlayerUnit = null;
    }

    private void SelectUnit(ISelectable unit)
    {
        selectedPlayerUnit = unit;
        unit.OnSelect();
    }
}