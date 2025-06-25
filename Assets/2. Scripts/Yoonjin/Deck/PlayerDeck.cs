using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerDeck
{
    // 선택한 카드덱
    [SerializeField]
    public List<EntryDeckData> deckDatas = new List<EntryDeckData>();
}
