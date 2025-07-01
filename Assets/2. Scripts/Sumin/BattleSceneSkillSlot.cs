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
    [SerializeField] private Image skillIconImage;
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI reuseNumberText;

    [Header("스킬 슬롯 뒷면 : 스킬 코스트(쿨타임)")]
    [SerializeField] private Button BackSkillBtn;
    [SerializeField] private TextMeshProUGUI skillCostText;
    [SerializeField] private GameObject lockImage;

    // 스킬 데이터들
    private SkillData selectedSkillData;
    private int currentskillIndex;
    private int coolDown;
    private int reuseNumber;

    public void Initialize(SkillData skillData, int index)
    {
        // skill data를 넣기
        if (skillData == null)
        {
            ToggleSkillSlot(false); // 사용 가능 여부에 따라 앞or뒤 켜고 끄기
            return;
        }

        ToggleSkillSlot(skillData.CheckCanUseSkill()); // 사용 가능 여부에 따라 앞or뒤 켜고 끄기

        selectedSkillData = skillData;
        currentskillIndex = index;
        coolDown = skillData.coolDown;
        reuseNumber = skillData.reuseCount;

        // UI에 반영
        skillCostText.text = $"{coolDown}";
        reuseNumberText.text = $"{reuseNumber}";
        skillIconImage.sprite = skillData.skillIcon;
        skillName.text = skillData.skillName;

        if (reuseNumber <=0)
        {
            LockSkill();
        }
    }

    // 스킬 버튼 누르면 이 스킬의 슬롯 인덱스에 맞춰서 반영하여 스킬 선택
    public void OnFrontSkillBtn()
    {
        InputManager.Instance.SelectSkill(currentskillIndex);

        // 선택한 스킬 넘겨주기
        InputManager.Instance.SelectedSkillData = selectedSkillData;
    }

    public void OnBackSkillBtn()
    {
        // 버튼 뒤쪽이 보이면 클릭 시 잠시 앞면 보여줌
        // 잠시 앞면 보일때 못누르게해야함.
    }

    // 스킬 슬롯 앞or뒤 토글
    private void ToggleSkillSlot(bool toggle)
    {
        FrontSkillBtn.gameObject.SetActive(toggle);
        BackSkillBtn.gameObject.SetActive(!toggle);
    }

    // 스킬 슬롯 비활성화
    private void LockSkill()
    {
        ColorBlock colorBlock = BackSkillBtn.colors;
        BackSkillBtn.interactable = false;
        colorBlock.normalColor = new Color(0, 0, 0);
        lockImage.SetActive(true);
        skillCostText.gameObject.SetActive(false);
    }
}