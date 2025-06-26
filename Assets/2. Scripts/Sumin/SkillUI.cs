using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    [SerializeField] private List<BattleSceneSkillSlot> skillSlot;

    public void UpdateSkillList(Unit selectedUnit)
    {
        // 유닛이 보유한 스킬 리스트들을 차례로 슬롯에 넣어주기? - 나중에

        //for (int i = 0; i < skillSlot.Count; i++)
        //{
        //    skillSlot[i].skill = selectedUnit.GetComponent<PlayerSkillController>().skills[i];
        //    skillSlot[i].Initialize(skills[i], i, OnSkillSlotClicked);
            
        //}
    }
}
