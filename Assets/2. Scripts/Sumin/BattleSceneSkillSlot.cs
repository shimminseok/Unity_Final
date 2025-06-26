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

    // 임시 설정값, 나중엔 선택한 유닛의 스킬 데이터에서 가져오기
    [SerializeField] private int skillCost;
    [SerializeField] private int reuseNumber;
    [SerializeField] private int skillIndex;

    private Action<SkillData> onSkillClicked;

    public void Initialize(Skill skill, int index, Action<SkillData> onClickCallback)
    {
        // skill data를 넣기
        skillCostText.text = $"{skillCost}";
        reuseNumberText.text = $"{reuseNumber}";
        skillIdText.text = $"{skillIndex}";

        FrontSkillBtn.onClick.RemoveAllListeners();
        FrontSkillBtn.onClick.AddListener(OnFrontSkillBtn);
    }

    public void OnFrontSkillBtn()
    {
        
        // 스킬 버튼 누르면 이 스킬의 슬롯 인덱스에 맞춰서 반영
        // selectedUnit.playerSkillController.ChangeSkill(skillIndex);
    }
    // 버튼 앞쪽이 보이면 스킬 선택 가능
    // 버튼 뒤쪽이 보이면 잠시 앞면 보여줌
}
