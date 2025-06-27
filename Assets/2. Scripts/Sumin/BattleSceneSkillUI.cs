using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSceneSkillUI : MonoBehaviour
{
    [SerializeField] private List<BattleSceneSkillSlot> skillSlot;

    //유닛이 보유한 스킬 리스트들을 차례로 슬롯에 넣어주기
    public void UpdateSkillList(Unit selectedUnit)
    {
        if (selectedUnit is PlayerUnitController playerUnit)
        {
            for (int i = 0; i < skillSlot.Count; i++)
            {
                skillSlot[i].Initialize(playerUnit.PlayerSkillController.skills[i], i);
            }
        } 
    }
}
