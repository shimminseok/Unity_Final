using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectMainUI : UIBase
{
    [Header("보유한 전체 캐릭터 영역")]
    [SerializeField] private Transform ownedCharacterParent;

    [SerializeField] private CharacterButton characterButtonPrefab;

    [Header("현재 선택한 캐릭터 영역")]
    [SerializeField] private Transform selectedCharacterParent;

    [Header("캐릭터 정보 패널")]
    [SerializeField] private CharacterInfoPanel characterInfoPanel;

    [Header("전투 시작")]
    [SerializeField] private Button startBattleButton;


    // 생성된 보유 캐릭터 & 선택 캐릭터 버튼들을 담는 리스트
    private List<CharacterButton> selectedCharacterButtons = new();

    // 보유 캐릭터 & 선택 캐릭터 SO들을 담는 리스트

    private Dictionary<int, CharacterButton> characterSlotDic = new();


    private void Start()
    {
        // TODO: 나중에 실제 보유한 캐릭터 데이터를 ownedCharacters에 할당한다

        GenerateOwnedCharacterButtons();
        startBattleButton.onClick.AddListener(OnClickStartBattle);
    }


    // 현재 보유 중인 캐릭터 목록 버튼 생성
    private void GenerateOwnedCharacterButtons()
    {
        // !!!임시로 더미 전체 캐릭터풀의 데이터 집어넣음!!!
        // 테이블 데이터에서 전체 캐릭터 목록 get table 로 받아옴

        var units = AccountManager.Instance.MyPlayerUnits;

        foreach (KeyValuePair<int, EntryDeckData> entryDeckData in units)
        {
            if (characterSlotDic.ContainsKey(entryDeckData.Key))
                continue;

            var slot = Instantiate(characterButtonPrefab, ownedCharacterParent);
            slot.Initialize(entryDeckData.Value.CharacterSo, false, OnCharacterButtonClicked);
            characterSlotDic.Add(entryDeckData.Key, slot);
        }
    }

    // 선택된 캐릭터 목록 버튼 생성
    private void GenerateSelectedCharacterButtons(List<EntryDeckData> selectedDeck)
    {
        // 1. 필요한 만큼 버튼 활성화 및 초기화
        for (int i = 0; i < selectedDeck.Count; i++)
        {
            CharacterButton btn;

            if (i < selectedCharacterButtons.Count)
            {
                btn = selectedCharacterButtons[i];
                btn.gameObject.SetActive(true);
            }
            else
            {
                btn = Instantiate(characterButtonPrefab, selectedCharacterParent);
                selectedCharacterButtons.Add(btn);
            }

            btn.Initialize(selectedDeck[i].CharacterSo, true, OnCharacterButtonClicked);
        }

        for (int i = selectedDeck.Count; i < selectedCharacterButtons.Count; i++)
        {
            selectedCharacterButtons[i].gameObject.SetActive(false);
        }
    }

    // 최근 선택된 캐릭터 정보를 패널에 갱신
    public void UpdateCharacterInfoPanel(PlayerUnitSO character)
    {
        // 현재 선택된 덱에서 찾음
        var entry = DeckSelectManager.Instance.GetSelectedDeck()
            .Find(entry => entry.CharacterSo == character);

        if (entry != null)
        {
            characterInfoPanel.SetData(entry);
        }
    }

    /// <summary>
    /// 이하 클릭 이벤트
    /// </summary>

    // 전투 시작 버튼 클릭
    private void OnClickStartBattle()
    {
        DeckSelectManager.Instance.ConfirmDeckAndStartBattle();
    }

    // 보유 캐릭터 버튼 클릭 처리
    // 선택 중인지에 따라 다른 처리
    private void OnCharacterButtonClicked(int id, bool isSelected)
    {
        var playerUnit = TableManager.Instance.GetTable<PlayerUnitTable>().GetDataByID(id);
        // 선택된 경우는 정보 갱신
        if (isSelected)
        {
            // 덱에 존재하는지 확인하고
            var entry = DeckSelectManager.Instance.GetSelectedDeck()
                .Find(entry => entry.CharacterSo.ID == id);

            if (entry != null)
            {
                // 현재 선택된 캐릭터로 갱신
                DeckSelectManager.Instance.SetCurrentSelectedCharacter(entry);

                UpdateCharacterInfoPanel(entry.CharacterSo);
            }
        }

        // 선택 안 된 경우는 선택 or 해제 처리
        else
        {
            DeckSelectManager.Instance.SelectCharacter(playerUnit);

            GenerateSelectedCharacterButtons(DeckSelectManager.Instance.GetSelectedDeck());
            UpdateCharacterInfoPanel(playerUnit);
        }
    }
}