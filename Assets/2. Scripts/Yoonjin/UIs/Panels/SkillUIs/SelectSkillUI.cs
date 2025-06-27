using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectSkillUI : UIBase
{
    [Header("보유 패시브 스킬 선택 영역")]
    [SerializeField] private Transform passiveSkillParent;

    [Header("보유 액티브 스킬 선택 영역")]
    [SerializeField] private Transform activeSkillParent;

    [Header("장착 스킬 슬롯")]
    [SerializeField] private Transform passiveSkillSlot;
    [SerializeField] private Transform[] activeSkillSlots;

    [Header("장착한 스킬 이름 / 설명 / 효과")]
    [SerializeField] private TMP_Text skillName;
    [SerializeField] private TMP_Text skillDescription;
    [SerializeField] private TMP_Text skillEffect;

    [Header("스킬 버튼 프리팹")]
    [SerializeField] private SkillButton skillButtonPrefab;

    // 현재 선택된 캐릭터
    private EntryDeckData currentCharacter;



    public override void Open()
    {
        base.Open();
        UpdateSkillUI();
    }

    // UI 갱신
    public void UpdateSkillUI()
    {
        currentCharacter = DeckSelectManager.Instance.GetCurrentSelectedCharacter();

        if (currentCharacter == null) return;

        skillName.text = "";
        skillDescription.text = "";
        skillEffect.text = "";

        ClearAllSlots();
        GenerateOwnedSkillButtons();
        GenerateSelectedSkillButtons();
    }

    // 전부 삭제
    private void ClearAllSlots()
    {
        DestroyChildren(passiveSkillParent);
        DestroyChildren(activeSkillParent);

        foreach (Transform t in activeSkillSlots)
            DestroyChildren(t);

        DestroyChildren(passiveSkillSlot);

    }

    // 보유 중인 스킬 버튼 생성
    /// <summary>
    /// !!! 임시조치로 현재 테이블에 있는 모든 스킬 가져옴!!!
    /// </summary>
    private void GenerateOwnedSkillButtons()
    {
        // 현재 선택한 캐릭터의 직업
        var job = currentCharacter.characterSO.JobType;

        // 테이블에서 액티브, 패시브 스킬 목록 가져오기
        var activeList = TableManager.Instance.GetTable<ActiveSkillTable>().GetActiveSkillsByJob(job);
        var passiveList = TableManager.Instance.GetTable<PassiveSkillTable>().GetPassiveSkillsByJob(job);

        foreach (var active in activeList)
        {
            var btn = Instantiate(skillButtonPrefab, activeSkillParent);
            btn.Initialize(active, false, OnSkillButtonClicked);
        }

        // 테이블에서 패시브 스킬 가져오기
        foreach (var passive in passiveList)
        {
            var btn = Instantiate(skillButtonPrefab, passiveSkillParent);
            btn.Initialize(passive, false, OnSkillButtonClicked);
        }
    }

    // 선택 중인 스킬 슬롯 채우기
    private void GenerateSelectedSkillButtons()
    {
        // 패시브 슬롯
        if (currentCharacter.passiveSkill != null)
        {
            var btn = Instantiate(skillButtonPrefab, passiveSkillSlot);
            Debug.Log("선택한 패시브 슬롯 할당");
            btn.Initialize(currentCharacter.passiveSkill, true, OnSkillButtonClicked);
        }

        // 액티브 슬롯
        for (int i = 0; i < currentCharacter.skillDatas.Length; i++)
        {
            var active = currentCharacter.skillDatas[i];
            if (active == null) continue;

            var btn = Instantiate(skillButtonPrefab, activeSkillSlots[i]);
            btn.Initialize(active, true, OnSkillButtonClicked);
        }
    }

    // 액티브 스킬 정보 표시
    private void ShowSkillInfo(ActiveSkillSO active)
    {
        skillName.text = active.name;
        // !!!아직 액티브 스킬 description이 없음!!!
        // skillDescription.text = active.description;
    }

    // 패시브 스킬 정보 표시
    private void ShowSkillInfo(PassiveSO passive)
    {
        skillName.text = passive.PassiveName;
        skillDescription.text = passive.Description;
    }

    // 클릭 콜백
    private void OnSkillButtonClicked(SkillButton btn, bool isSelected)
    {
        // 장착된 슬롯에서 클릭 시 정보 패널에 설명 표시
        if(isSelected)
        {
            if (btn.IsPassive)
                ShowSkillInfo(btn.GetPassiveSkill());
            else
                ShowSkillInfo(btn.GetActiveSkill());

            return;
        }

        // 보유 스킬 클릭 시 장착 / 해제
        else
        {
            if (btn.IsPassive)
                DeckSelectManager.Instance.SelectPassiveSkill(btn.GetPassiveSkill());
            else
                DeckSelectManager.Instance.SelectActiveSkill(btn.GetActiveSkill());

            // UI 갱신
            UpdateSkillUI();
        }
    }

    // 초기화: 자식 오브젝트 삭제 (SkillInfo 진입할 때)
    private void DestroyChildren (Transform parent)
    {
        foreach(Transform child in parent)
        {
            Destroy(child.gameObject); 
        }
    }

}
