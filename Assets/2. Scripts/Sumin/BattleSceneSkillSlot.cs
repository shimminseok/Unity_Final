using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleSceneSkillSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillCostText;
    [SerializeField] private TextMeshProUGUI reuseNumberText;
    [SerializeField] private Button FrontSkillBtn;
    [SerializeField] private Button BackSkillBtn;

    // UI에 들어온 스킬 확인용
    [SerializeField] private TextMeshProUGUI skillIdText;

    private int currentskillIndex;
    private int cost;
    private int reuseNumber;

    public void Initialize(Skill skill, int index)
    {
        // skill data를 넣기
        this.currentskillIndex = index;
        this.cost = skill.cost;
        this.reuseNumber = skill.reuseMaxNumber;

        skillCostText.text = $"{cost}";
        reuseNumberText.text = $"{reuseNumber}";
        skillIdText.text = $"{currentskillIndex}";
    }

    public void OnFrontSkillBtn()
    {
        // 스킬 버튼 누르면 이 스킬의 슬롯 인덱스에 맞춰서 반영
        InputManager.Instance.SelectSkill(currentskillIndex);
    }
    // 버튼 앞쪽이 보이면 스킬 선택 가능
    // 버튼 뒤쪽이 보이면 잠시 앞면 보여줌
}
