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
    private List<CharacterButton> ownedCharacterButtons = new();
    private List<CharacterButton> selectedCharacterButtons = new();

    // 보유 캐릭터 & 선택 캐릭터 SO들을 담는 리스트
    private List<PlayerUnitSO> ownedCharacters = new();
    private List<PlayerUnitSO> selectedCharacters = new();



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
        var table = TableManager.Instance.GetTable<PlayerUnitTable>();
        ownedCharacters.Clear();

        // ID 순회: 현재 캐릭터 id가 1부터 7까지이므로 임시 조치
        for (int id = 1; id <= 20; id++)
        {
            var unit = table.GetDataByID(id);

            if (unit != null)
            {
                ownedCharacters.Add(unit);
            }
        }

        foreach (PlayerUnitSO unit in ownedCharacters)
        {
            CharacterButton btn = Instantiate(characterButtonPrefab, ownedCharacterParent);
            btn.Initialize(unit, false, OnCharacterButtonClicked);
            ownedCharacterButtons.Add(btn);
        }
    }

    // 선택된 캐릭터 목록 버튼 생성
    private void GenerateSelectedCharacterButtons(List<EntryDeckData> selectedDeck)
    {
        // 기존 버튼 모두 제거
        foreach (var btn in selectedCharacterButtons)
            Destroy(btn.gameObject);
        selectedCharacterButtons.Clear();

        // 새로운 버튼 생성
        foreach(var entry in selectedDeck)
        {
            var btn = Instantiate(characterButtonPrefab, selectedCharacterParent);
            btn.Initialize(entry.characterSO, true, OnCharacterButtonClicked);
            selectedCharacterButtons.Add(btn);
        }
    }

    // 최근 선택된 캐릭터 정보를 패널에 갱신
    private void UpdateCharacterInfoPanel(PlayerUnitSO character)
    {
        var entry = DeckSelectManager.Instance.GetSelectedDeck()
            .Find(entry => entry.characterSO == character);

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
    private void OnCharacterButtonClicked(PlayerUnitSO character, bool isSelected)
    {
        // 선택된 경우는 정보 갱신
        if(isSelected)
        {
            UpdateCharacterInfoPanel(character);
        }

        // 선택 안 된 경우는 선택 or 해제 처리
        else
        {
            DeckSelectManager.Instance.SelectCharacter(character);

            GenerateSelectedCharacterButtons(DeckSelectManager.Instance.GetSelectedDeck());
            UpdateCharacterInfoPanel(character);
        }
    }

}
