using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelectSkillUI : UIBase
{
    [Header("보유 패시브 스킬 선택 영역")]
    [SerializeField] private Transform passiveSkillParent;

    [Header("보유 액티브 스킬 선택 영역")]
    [SerializeField] private Transform activeSkillParent;

    [Header("장착한 4개의 스킬 영역")]
    [SerializeField] private Transform selectedSkillParent;

    [Header("장착한 스킬 이름 / 설명 / 효과")]
    [SerializeField] private TMP_Text skillName;
    [SerializeField] private TMP_Text skillDescription;
    [SerializeField] private TMP_Text skillEffect;

    [Header("스킬 버튼 프리팹")]
    [SerializeField] private SkillButton skillButtonPrefab;



    // 현재 선택된 캐릭터가 가지고 있는 스킬 목록 버튼 만들기
    private void GenarateSkillButtons()
    {
        var entry = DeckSelectManager.Instance.GetCurrentSelectedCharacter();
        if (entry == null) return;

        // 스킬 영역 초기화
        foreach (Transform t in passiveSkillParent) Destroy(t.gameObject);
        foreach (Transform t in activeSkillParent) Destroy(t.gameObject);

        // 캐릭터 JobType에 맞춰 스킬 리스트를 받아옴
        var passiveList = TableManager.Instance.GetTable<PassiveSkillTable>().GetPassiveSkillsByJob(entry.characterSO.JobType);

        // 패시브 스킬 버튼 생성
        foreach (var passive in passiveList)
        {
            var btn = Instantiate(skillButtonPrefab, passiveSkillParent);
            btn.SetPassiveSkill(passive);
        }

        var activeList = TableManager.Instance.GetTable<ActiveSkillTable>().GetActiveSkillsByJob(entry.characterSO.JobType);

        // 액티브 스킬 버튼 생성
        foreach (var active in activeList)
        {

        }
    }
}
