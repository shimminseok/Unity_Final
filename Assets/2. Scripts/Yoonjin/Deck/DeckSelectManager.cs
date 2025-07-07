using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeckSelectManager : SceneOnlySingleton<DeckSelectManager>
{
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
        var alreadySelected = selectedDeck.Find(alreadySelected => alreadySelected.CharacterSo == newCharacterSO);

        if (alreadySelected != null)
        {
            selectedDeck.Remove(alreadySelected); // 이미 선택한 캐릭터면 해제
            currentSelectedCharacter = null;
        }

        else if (selectedDeck.Count < maxCharacterCount)
        {
            // 새로운 캐릭터 데이터 추가
            EntryDeckData newEntry = new EntryDeckData(newCharacterSO.ID);

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
                currentSelectedCharacter.InvokeSkillChanged();
                return;
            }
        }

        // 비어 있는 슬롯에 장착
        for (int i = 0; i < skills.Length; i++)
        {
            if (skills[i] == null)
            {
                skills[i] = activeSkill;
                currentSelectedCharacter.InvokeSkillChanged();
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

        // 이미 선택했으면 해제
        if (currentSelectedCharacter.passiveSkill == passive)
        {
            currentSelectedCharacter.passiveSkill = null;
        }


        else
        {
            currentSelectedCharacter.passiveSkill = passive;
        }
    }

    // 캐릭터에 장비 장착
    public void SelectEquipment(EquipmentItem equip)
    {
        if (currentSelectedCharacter == null)
            return;

        // 장비 타입 받아옴
        EquipmentType type     = equip.EquipmentItemSo.EquipmentType;
        var           equipped = currentSelectedCharacter.equippedItems;

        // 현재 type 슬롯에 장착된 아이템
        if (equipped.TryGetValue(type, out var currentEquipped))
        {
            // 같은 아이템을 다시 클릭한 경우에 해제
            if (currentEquipped == equip)
            {
                equip.IsEquipped = false;
                equipped.Remove(type);

                // 디버깅용
                currentSelectedCharacter.SyncDebugEquipments();
                currentSelectedCharacter.InvokeEquipmentChanged();
                return;
            }

            // 다른 장비로 교체하며 기존 장비 해제
            currentEquipped.IsEquipped = false;
        }

        // 새 장비 장착
        equip.IsEquipped = true;
        equipped[type] = equip;

        // 디버깅용
        currentSelectedCharacter.SyncDebugEquipments();
        currentSelectedCharacter.InvokeEquipmentChanged();
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