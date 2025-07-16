using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIDeckBuilding : UIBase
{
    [Header("보유한 전체 캐릭터 영역")]
    [SerializeField] private Transform ownedCharacterParent;

    [FormerlySerializedAs("characterButtonPrefab")]
    [SerializeField] private UnitSlot unitSlotPrefab;


    // 보유 캐릭터 & 선택 캐릭터 SO들을 담는 리스트
    private Dictionary<int, UnitSlot> characterSlotDic = new();


    private bool isChangeDeck;


    // 현재 보유 중인 캐릭터 목록 버튼 생성
    private void GenerateOwnedCharacterButtons()
    {
        var units = AccountManager.Instance.MyPlayerUnits;

        foreach (KeyValuePair<int, EntryDeckData> entryDeckData in units)
        {
            if (characterSlotDic.ContainsKey(entryDeckData.Key))
                continue;

            UnitSlot slot = Instantiate(unitSlotPrefab, ownedCharacterParent);
            slot.Initialize(entryDeckData.Value);
            characterSlotDic.Add(entryDeckData.Key, slot);
            slot.OnClickSlot += OnCharacterButtonClicked;
        }
    }

    // 선택된 캐릭터 목록 버튼 생성
    private void GenerateSelectedCharacterButtons(List<EntryDeckData> selectedDeck)
    {
        int index = 0;
        foreach (var entry in selectedDeck)
        {
            if (entry == null)
            {
                index++;
                continue;
            }

            AvatarPreviewManager.Instance.ShowAvatar(index++, entry.CharacterSo.JobType);
        }
    }

    // 최근 선택된 캐릭터 정보를 패널에 갱신
    public void UpdateCharacterInfoPanel(PlayerUnitSO character)
    {
        // 현재 선택된 덱에서 찾음
        var entry = DeckSelectManager.Instance.GetSelectedDeck()
            .Find(entry => entry.CharacterSo.ID == character.ID);
    }

    /// <summary>
    /// 이하 클릭 이벤트
    /// </summary>

    // 보유 캐릭터 버튼 클릭 처리
    private void OnCharacterButtonClicked(EntryDeckData data)
    {
        isChangeDeck = true;
        if (data.IsCompeted)
        {
            DeckSelectManager.Instance.RemoveUnitInDeck(data);
            AvatarPreviewManager.Instance.HideAvatar(data.CharacterSo.JobType);
        }
        else
        {
            DeckSelectManager.Instance.AddUnitInDeck(data, out int index);
            if (index == -1)
                return;
            AvatarPreviewManager.Instance.ShowAvatar(index, data.CharacterSo.JobType);
        }
    }

    public override void Open()
    {
        base.Open();
        GenerateOwnedCharacterButtons();
        GenerateSelectedCharacterButtons(DeckSelectManager.Instance.GetSelectedDeck());
        isChangeDeck = false;
    }

    public override void Close()
    {
        base.Close();
    }
}