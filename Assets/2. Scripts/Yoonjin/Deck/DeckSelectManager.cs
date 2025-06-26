using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeckSelectManager : SceneOnlySingleton<DeckSelectManager>
{
    // UI 연동해서 테스트해보려는데
    // 내가 갖고있는 캐릭터풀이 < 어디서?.? < 아직 미정
    // 버튼 스크립트 만들어서 PlayerUnitSo / SelectCharacter 호출 < 테스트는 걍 박아놓기


    // 선택된 캐릭터와 스킬 목록
    [SerializeField]
    private List<EntryDeckData> selectedDeck = new List<EntryDeckData>();

    // 최근에 선택한 캐릭터
    private EntryDeckData currentSelectedCharacter;

    // 최대 선택 가능한 캐릭터 수
    private const int maxCharacterCount = 4;


    #region getter
    public List<EntryDeckData> GetSelectedDeck()
    {
        return selectedDeck;
    }
    #endregion

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    // 캐릭터 선택 시
    public void SelectCharacter(PlayerUnitSO newCharacterSO)
    {
        // 이미 선택됐는지 확인
        var alreadySelected = selectedDeck.Find(alreadySelected => alreadySelected.characterSO == newCharacterSO);

        if (alreadySelected != null)
        {
            selectedDeck.Remove(alreadySelected);   // 이미 선택한 캐릭터면 해제
            currentSelectedCharacter = null;
        }

        else if (selectedDeck.Count < maxCharacterCount)
        {
            // 새로운 캐릭터 데이터 추가
            EntryDeckData newEntry = new EntryDeckData()
            {
                characterSO = newCharacterSO
            };

            selectedDeck.Add(newEntry);
            currentSelectedCharacter = newEntry;
        }
    }


    // 캐릭터에 스킬 장착
    // 캐릭터, 스킬, 스킬 슬롯 번호
    public void SelectSkill(SkillData skill, int slotIndex)
    {
        if (currentSelectedCharacter == null) return;

        // 이미 선택한 스킬인지
        bool isAlreadyEquipped = currentSelectedCharacter.skillDatas.Contains(skill);

        if (isAlreadyEquipped)
        {
            currentSelectedCharacter.skillDatas[slotIndex] = null;
            return;
        }

        // 다른 슬롯에 장착되어 있는 동일한 스킬은 제거
        for (int i = 0; i < currentSelectedCharacter.skillDatas.Length; i++)
        {
            if (currentSelectedCharacter.skillDatas[i] == skill)
            {
                currentSelectedCharacter.skillDatas[i] = null;
                break;
            }
        }

        // 최근 선택한 캐릭터의 스킬 추가
        currentSelectedCharacter.skillDatas[slotIndex] = skill;

    }

    // 캐릭터에 장비 장착
    public void SelectEquipment (EquipmentItemSO equip)
    {

    }

    // 게임 시작 버튼 클릭 시 호출
    // 덱을 PlayerDeckContainer에 저장하고, 배틀씬으로 이동한다
    public void ConfirmDeckAndStartBattle()
    {
        PlayerDeckContainer.Instance.SetDeck(selectedDeck);
        LoadSceneManager.Instance.LoadScene("BattleScene");
    }

    // 덱 전체 초기화
    public void ResetDeck()
    {
        selectedDeck.Clear();
        PlayerDeckContainer.Instance.ClearDeck();
    }
}
