using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    [Header("이미지 / 코스트")]
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text cost;
    [SerializeField] private Button button;

    // 현재 버튼에 할당된 스킬 데이터
    private ActiveSkillSO activeActiveSkill;
    private PassiveSO passiveSkill;

    // 현재 버튼의 역할이 패시브인지 액티브인지
    private bool isPassive;

    // 선택된 스킬인지
    private bool isSelected;

    private Action<SkillButton, bool> onClick;



    // 액티브 스킬 초기화
    public void Initialize(ActiveSkillSO active, bool isSelectedSkill, Action<SkillButton, bool> callback)
    {
        activeActiveSkill = active;
        isPassive = false;
        this.isSelected = isSelectedSkill;
        onClick = callback;

        icon.sprite = active.skillIcon;
        cost.text = active.coolTime.ToString();

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClick);

        Debug.Log($"[Initialize] 액티브 스킬: {active.name}, 코스트: {active.coolTime}");
    }

    // 패시브 스킬 초기화
    public void Initialize(PassiveSO passive, bool isSelectedSkill, Action<SkillButton, bool> onClick)
    {
        passiveSkill = passive;
        isPassive = true;
        this.isSelected = isSelectedSkill;
        this.onClick = onClick;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClick);

        Debug.Log($"[Initialize] 패시브 스킬: {passive.name}");

        //!!!아직 패시브 스킬에는 아이콘이 없음!!!
        //icon.sprite = passive.skillIcon;
    }


    public void OnClick()
    {
        Debug.Log($"[SkillButton] 클릭됨 - isSelected: {isSelected}, isPassive: {isPassive}");
        onClick?.Invoke(this, isSelected);
    }

    #region
    public ActiveSkillSO GetActiveSkill() => activeActiveSkill;
    public PassiveSO GetPassiveSkill() => passiveSkill;
    public bool IsPassive => isPassive;
    #endregion
}
