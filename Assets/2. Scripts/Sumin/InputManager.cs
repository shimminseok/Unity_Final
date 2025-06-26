using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. 플레이어 유닛 선택 (아군만)
// 2. 플레이어 유닛이 가지고 있는 스킬을 받아옴 -> 스킬 선택
//Onclick 안하고 UI에서 선택 스킬이 받아지면? 넘겨줌
// 3. 스킬의 타겟Type을 받아와서 레이어를 정해줌 -> 타겟(아군/적) 유닛 선택

// 나중에 Enum으로 이동
public enum InputPhase
{
    SelectUnit,     // 플레이어 유닛 선택 상태
    SelectSkill,    // 유닛이 사용할 스킬 선택 상태
    SelectTarget    // 유닛이 스킬을 사용할 타겟 선택 상태
}

// 전투 씬에서 플레이어 선택 Input 관리
public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera mainCam;
    [SerializeField] private LayerMask unitLayer;
    [SerializeField] private LayerMask playerUnitLayer;
    [SerializeField] private LayerMask enemyUnitLayer;
    private ISelectable selectedUnit;
    private InputPhase currentPhase = InputPhase.SelectUnit;
    public InputPhase CurrentPhase => currentPhase;
    private SkillData selectedSkill;

    void Start()
    {
        if (mainCam == null)
        {
            mainCam = Camera.main;
        }
    }
    
    void Update()
    {
        // 각 Phase별 초기화 또는 상태 진입 처리
        switch (currentPhase)
        {
            case InputPhase.SelectUnit:
                OnClickPlayerUnit();
                break;
            case InputPhase.SelectSkill:
                // 스킬 UI 선택
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
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, playerUnitLayer))
            {
                ISelectable selectable = hit.transform.GetComponent<ISelectable>();
                SelectUnit(selectable);
                currentPhase = InputPhase.SelectSkill;
            }
        }
    }

    // 스킬을 사용할 타겟 유닛 선택
    private void OnClickTargetUnit()
    {
        LayerMask targetLayer = GetTargetLayerMask(selectedSkill.selectCamp);
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, playerUnitLayer))
            {
                ISelectable selectable = hit.transform.GetComponent<ISelectable>();
                SelectUnit(selectedUnit);
            }
            else
            {
                DeselectUnit();
            }
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
        if (selectedUnit != null)
            selectedUnit.OnDeselect();

        selectedUnit = null;
    }
    void SelectUnit(ISelectable unit)
    {
        DeselectUnit();
        selectedUnit = unit;
        unit.OnSelect();
    }
}
