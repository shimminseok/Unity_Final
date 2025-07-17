using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
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


    private List<SkillButton> selectedSkillButtonPool = new(); // 풀 추가
    private List<SkillButton> ownedSkillButtonPool = new();

    public event Action<EntryDeckData> OnSkillChanged;

    public override void Open()
    {
        base.Open();
        UpdateSkillUI();

        if (currentCharacter != null)
        {
            AvatarPreviewManager.Instance.ShowAvatar(currentCharacter.CharacterSo);
        }
    }

    public void SetCurrentSelectedUnit(EntryDeckData currentUnit)
    {
        DeckSelectManager.Instance.SetCurrentSelectedCharacter(currentUnit);
        currentCharacter = currentUnit;
    }

    public override void Close()
    {
        base.Close();
        OnSkillChanged?.Invoke(currentCharacter);
        AvatarPreviewManager.Instance.HideAllAvatars();
    }


    // UI 갱신
    public void UpdateSkillUI()
    {
        currentCharacter = DeckSelectManager.Instance.GetCurrentSelectedCharacter();

        if (currentCharacter == null) return;

        ClearSkillInfo();
        GenerateOwnedSkillButtons();
        GenerateSelectedSkillButtons();
    }


    // 보유 중인 스킬 버튼 생성
    /// <summary>
    /// !!! 임시조치로 현재 테이블에 있는 모든 스킬 가져옴!!!
    /// </summary>
    private void GenerateOwnedSkillButtons()
    {
        int poolIndex = 0;
        var job       = currentCharacter.CharacterSo.JobType;

        // var activeList = AccountManager.Instance.MySkills.Values;
        var activeList = TableManager.Instance.GetTable<ActiveSkillTable>().GetActiveSkillsByJob(job);
        foreach (var active in activeList)
        {
            var btn = GetOrCreateSkillButton(poolIndex++, activeSkillParent, ownedSkillButtonPool, skillButtonPrefab);
            btn.Initialize(active, false, OnSkillButtonClicked);
        }

        DisableRemainingButtons(poolIndex, ownedSkillButtonPool);
    }

    // 선택 중인 스킬 슬롯 채우기
    private void GenerateSelectedSkillButtons()
    {
        int poolIndex = 0;

        // 패시브
        if (currentCharacter.CharacterSo.PassiveSkill != null)
        {
            var btn = GetOrCreateSkillButton(poolIndex++, passiveSkillSlot, selectedSkillButtonPool, skillButtonPrefab);
            btn.Initialize(currentCharacter.CharacterSo.PassiveSkill, true, OnSkillButtonClicked);
        }

        // 액티브
        for (int i = 0; i < currentCharacter.skillDatas.Length; i++)
        {
            var active = currentCharacter.skillDatas[i];
            if (active == null) continue;

            var btn = GetOrCreateSkillButton(poolIndex++, activeSkillSlots[i], selectedSkillButtonPool, skillButtonPrefab);
            btn.Initialize(active, true, OnSkillButtonClicked);
        }

        DisableRemainingButtons(poolIndex, selectedSkillButtonPool);
    }

    private SkillButton GetOrCreateSkillButton(int index, Transform parent, List<SkillButton> pool, SkillButton prefab)
    {
        SkillButton btn;

        if (index < pool.Count)
        {
            btn = pool[index];
        }
        else
        {
            btn = Instantiate(prefab, parent);
            pool.Add(btn);
        }

        btn.transform.SetParent(parent, false); // false로 localPosition 유지
        btn.gameObject.SetActive(true);
        return btn;
    }

    private void DisableRemainingButtons(int fromIndex, List<SkillButton> pool)
    {
        for (int i = fromIndex; i < pool.Count; i++)
        {
            pool[i].gameObject.SetActive(false);
        }
    }

    // 액티브 스킬 정보 표시
    private void ShowSkillInfo(ActiveSkillSO active)
    {
        skillName.text = active.skillName;
        skillDescription.text = active.skillDescription;
    }

    // 패시브 스킬 정보 표시
    private void ShowSkillInfo(PassiveSO passive)
    {
        skillName.text = passive.PassiveName;
        skillDescription.text = passive.Description;
    }

    // 스킬 정보 리셋
    private void ClearSkillInfo()
    {
        skillName.text = "";
        skillDescription.text = "";
        skillEffect.text = "";
    }

    // 클릭 콜백
    private void OnSkillButtonClicked(SkillButton btn, bool isSelected)
    {
        ClearSkillInfo();

        // 장착된 슬롯에서 클릭 시 정보 패널에 설명 표시
        if (isSelected)
        {
            if (btn.IsPassive)
                ShowSkillInfo(btn.GetPassiveSkill());
            else
                ShowSkillInfo(btn.GetActiveSkill());
        }
        // 보유 스킬 클릭 시 장착 / 해제
        else
        {
            DeckSelectManager.Instance.SelectActiveSkill(btn.GetActiveSkill());
            // UI 갱신
            UpdateSkillUI();
        }
    }
}