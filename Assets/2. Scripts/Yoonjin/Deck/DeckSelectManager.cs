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

    // 최근에 덱에 넣은 캐릭터
    private EntryDeckData currentSelectedCharacter;

    // 최대 선택 가능한 캐릭터 수
    private const int maxCharacterCount = 4;


    #region getter
    public List<EntryDeckData> GetSelectedDeck()
    {
        return selectedDeck;
    }

    public EntryDeckData GetCurrentSelectedCharacter()
    {
        return currentSelectedCharacter;
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



    // 현재 편집 중인 캐릭터
    public void SetCurrentSelectedCharacter(EntryDeckData entry)
    {
        currentSelectedCharacter = entry;
    }

    // 덱에 캐릭터 넣기
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


    // 캐릭터에 액티브 스킬 장착 & 해제
    public void SelectActiveSkill(ActiveSkillSO activeSkill)
    {
        if (currentSelectedCharacter == null) return;

        var skills = currentSelectedCharacter.skillDatas;

        // 이미 장착된 스킬일 경우 해제
        for (int i = 0; i < skills.Length; i++)
        {
            if (skills[i] == activeSkill)
            {
                skills[i] = null;
                return;
            }
        }

        // 비어 있는 슬롯에 장착
        for (int i = 0; i < skills.Length; i++)
        {
            if (skills[i] == null)
            {
                skills[i] = activeSkill;
                return;
            }
        }

    }

    // 캐릭터에 패시브 스킬 장착 (1개만)
    public void SelectPassiveSkill(PassiveSO passive)
    {
        if (currentSelectedCharacter == null)
        {
            Debug.LogWarning("캐릭터 없음");
            return;
        }

        Debug.Log($"[SelectPassiveSkill] 시도: {passive.PassiveName}");

        // 이미 선택했으면 해제
        if (currentSelectedCharacter.passiveSkill == passive)
        {
            Debug.Log("해제됨");
            currentSelectedCharacter.passiveSkill = null;
        }


        else
        {
            Debug.Log("장착됨");
            currentSelectedCharacter.passiveSkill = passive;
        }

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
        LoadSceneManager.Instance.LoadScene("BattleScene_Main");
    }

    // 덱 전체 초기화
    public void ResetDeck()
    {
        selectedDeck.Clear();
        PlayerDeckContainer.Instance.ClearDeck();
    }
}
