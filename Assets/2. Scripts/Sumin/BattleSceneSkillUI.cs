using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSceneSkillUI : UIBase
{
    [SerializeField] private List<BattleSceneSkillSlot> skillSlot;
    [SerializeField] private BattleSceneAttackSlot attackSlot;

    //유닛이 보유한 스킬 리스트들을 차례로 슬롯에 넣어주기
    public void UpdateSkillList(Unit selectedUnit)
    {
        UIManager.Instance.Open<BattleSceneSkillUI>();
        if (selectedUnit is PlayerUnitController playerUnit)
        {
            for (int i = 0; i < skillSlot.Count; i++)
            {
                skillSlot[i].Initialize(playerUnit.SkillController.skills[i], i);
            }
        }
    }

    public void HighlightSkillSlot(int index)
    {
        for (int i = 0; i < skillSlot.Count; i++)
        {
            skillSlot[i].HighlightSkillBtn();
        }
    }
    public void HighlightBasicAttack()
    {
        attackSlot.HighlightAttackBtn();
    }
}