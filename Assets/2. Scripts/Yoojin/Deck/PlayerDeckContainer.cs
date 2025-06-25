using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeckContainer : Singleton<PlayerDeckContainer>
{
    // 인스펙터 표시 테스트 코드
    [SerializeField]
    private PlayerDeck debugDeck = new PlayerDeck();

    // 현재 덱
    public PlayerDeck CurrentDeck { get; private set; } = new PlayerDeck();

    protected override void Awake()
    {
        base.Awake();
    }

    // 덱 세팅
    public void SetDeck(List<EntryDeckData> selectedDeck)
    {
        CurrentDeck.deckDatas = selectedDeck;
        debugDeck.deckDatas = selectedDeck;
    }

    // 덱 초기화
    public void ClearDeck()
    {
        CurrentDeck.deckDatas.Clear();
        debugDeck.deckDatas.Clear();
    }
}
