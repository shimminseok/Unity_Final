using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : UIBase
{
    [Header("임시 전체 캐릭터 풀")]
    [SerializeField] private DummyCharacterLoader dummyPool;

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

    // 현재 보유 중인 캐릭터 목록을 버튼 생성
    private void GenerateOwnedCharacterButtons()
    {
        // !!!임시로 더미 전체 캐릭터풀의 데이터 집어넣음!!!
        // 테이블 데이터에서 get table 로 받아옴
        ownedCharacters = dummyPool.OwnedCharacters;

        foreach (PlayerUnitSO unit in ownedCharacters)
        {
            CharacterButton btn = Instantiate(characterButtonPrefab, ownedCharacterParent);
            btn.Initialize(unit, OnCharacterButtonClicked);
            ownedCharacterButtons.Add(btn);
        }
    }

    // 전투 시작 버튼 클릭
    private void OnClickStartBattle()
    {
        DeckSelectManager.Instance.ConfirmDeckAndStartBattle();
    }

    // 보유 캐릭터 버튼 클릭 처리
    private void OnCharacterButtonClicked(PlayerUnitSO character)
    {
        DeckSelectManager.Instance.SelectCharacter(character);
    }

    // 선택된 캐릭터 버튼 클릭 시 상세 정보 표시
    private void OnSelectedCharacterButtonClicked(PlayerUnitSO character)
    {

    }

    // 최근 선택된 캐릭터 정보를 패널에 갱신
    private void UpdateCharacterInfoPanel()
    {

    }
}
