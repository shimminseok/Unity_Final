using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleSceneSkillSlot : MonoBehaviour
{
    [Header("스킬 슬롯 앞면 : 남은 재사용 횟수")]
    [SerializeField] private Button FrontSkillBtn;
    [SerializeField] private TextMeshProUGUI reuseNumberText;

    [Header("스킬 슬롯 뒷면 : 스킬 코스트(쿨타임)")]
    [SerializeField] private Button BackSkillBtn;
    [SerializeField] private TextMeshProUGUI skillCostText;

    // Test : UI에 들어온 스킬 index 확인용
    [SerializeField] private TextMeshProUGUI skillIdText;

    // 스킬 데이터들
    private SkillData selectedSkillData;
    private int currentskillIndex;
    private int coolDown;
    private int reuseNumber;

    public void Initialize(SkillData skillData, int index)
    {
        // skill data를 넣기
        selectedSkillData = skillData;
        currentskillIndex = index;
        coolDown = skillData.coolDown;
        reuseNumber = skillData.reuseCount;

        // UI에 반영
        skillCostText.text = $"{coolDown}";
        reuseNumberText.text = $"{reuseNumber}";
        skillIdText.text = $"{currentskillIndex}";
    }

    // 스킬 버튼 누르면 이 스킬의 슬롯 인덱스에 맞춰서 반영하여 스킬 선택
    public void OnFrontSkillBtn()
    {
        InputManager.Instance.SelectSkill(currentskillIndex);

        // 선택한 스킬 넘겨주기
        InputManager.Instance.SelectedSkillData = selectedSkillData;
    }

    // 버튼 앞쪽이 보이면 클릭 시 스킬 선택 가능
    // 버튼 뒤쪽이 보이면 클릭 시 잠시 앞면 보여줌
}
