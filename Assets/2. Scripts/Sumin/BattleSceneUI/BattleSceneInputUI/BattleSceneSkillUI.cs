using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSceneSkillUI : UIBase
{
    [SerializeField] private List<BattleSceneSkillSlot> skillSlot;
    [SerializeField] private BattleSceneAttackSlot attackSlot;

    // Skill 선택 Exit 버튼
    public void OnClickSkillExit()
    {
        InputManager.Instance.OnClickSkillExitButton();
    }

    //유닛이 보유한 스킬 리스트들을 차례로 슬롯에 넣어주기
    public void UpdateSkillList(Unit selectedUnit)
    {
        UIManager.Instance.Open(this);
        if (selectedUnit is PlayerUnitController playerUnit)
        {
            for (int i = 0; i < skillSlot.Count; i++)
            {
                skillSlot[i].Initialize(playerUnit.SkillController.skills[i], i);
            }
        }
    }

    // 스킬 선택중 표시
    public void ToggleHighlightSkillSlot(bool toggle, int index)
    {
        skillSlot[index].ToggleHighlightSkillBtn(toggle);
    }

    public void ToggleHighlightBasicAttack(bool toggle)
    {
        attackSlot.ToggleHighlightAttackBtn(toggle);
    }
}