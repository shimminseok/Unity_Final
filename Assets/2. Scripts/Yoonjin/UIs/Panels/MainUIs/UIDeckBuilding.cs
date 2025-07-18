using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIDeckBuilding : UIBase
{
    [Header("보유한 전체 캐릭터 영역")]
    [SerializeField] private Transform ownedCharacterParent;

    [SerializeField] private List<CompeteUnitSlot> competedUnitSlots;

    [FormerlySerializedAs("characterButtonPrefab")]
    [SerializeField] private UnitSlot unitSlotPrefab;

    [Header("UnitInfoPanel")]
    [SerializeField] private PanelSelectedUnitInfo unitInfoPanel;

    // 보유 캐릭터 & 선택 캐릭터 SO들을 담는 리스트
    private Dictionary<int, UnitSlot> characterSlotDic = new();


    private UnitSlot selectedUnitSlot;
    private AvatarPreviewManager avatarPreviewManager => AvatarPreviewManager.Instance;

    // 현재 보유 중인 캐릭터 목록 버튼 생성
    private void GenerateHasUnitSlots()
    {
        var units = AccountManager.Instance.MyPlayerUnits;

        foreach (KeyValuePair<int, EntryDeckData> entryDeckData in units)
        {
            if (characterSlotDic.ContainsKey(entryDeckData.Key))
                continue;

            UnitSlot slot = Instantiate(unitSlotPrefab, ownedCharacterParent);
            slot.Initialize(entryDeckData.Value);
            characterSlotDic.Add(entryDeckData.Key, slot);
            slot.OnClicked += OnClickedHasUnitSlot;
            slot.OnHeld += OnHeldHasUnitSlot;
        }
    }

    // 선택된 캐릭터 목록
    private void ShowCompetedUnit(List<EntryDeckData> selectedDeck)
    {
        int index = 0;
        foreach (EntryDeckData entry in selectedDeck)
        {
            if (entry == null)
            {
                index++;
                continue;
            }

            competedUnitSlots[index].SetCompeteUnitData(entry);
            avatarPreviewManager.ShowAvatar(index++, entry.CharacterSo.JobType);
        }
    }

    // 보유 캐릭터 버튼 클릭 처리
    private void OnClickedHasUnitSlot(EntryDeckData data)
    {
        if (!data.IsCompeted)
        {
            DeckSelectManager.Instance.AddUnitInDeck(data, out int index);
            if (index == -1)
                return;
            competedUnitSlots[index].SetCompeteUnitData(data);
            avatarPreviewManager.ShowAvatar(index, data.CharacterSo.JobType);
        }
        else
        {
            characterSlotDic[data.CharacterSo.ID].SetSelectedMarker(false);
        }
    }

    private void OnHeldHasUnitSlot(EntryDeckData data)
    {
        unitInfoPanel.SetInfoPanel(data);
        unitInfoPanel.OpenPanel();
    }

    public void RemoveUnitInDeck(EntryDeckData data)
    {
        characterSlotDic[data.CharacterSo.ID].SetCompetedMarker(false);
        DeckSelectManager.Instance.RemoveUnitInDeck(data);
        avatarPreviewManager.HideAvatar(data.CharacterSo.JobType);
    }

    public void SetSelectedUnitSlot(UnitSlot slot)
    {
        if (slot != selectedUnitSlot)
        {
            selectedUnitSlot?.Deselect();
        }

        selectedUnitSlot = slot;
        selectedUnitSlot.SetSelectedMarker(true);
    }

    public override void Open()
    {
        base.Open();
        GenerateHasUnitSlots();
        ShowCompetedUnit(DeckSelectManager.Instance.GetSelectedDeck());
    }

    public override void Close()
    {
        base.Close();

        foreach (var slot in characterSlotDic.Values)
        {
            slot.Deselect();
        }

        avatarPreviewManager.HideAllBuilindUIAvatars();
        selectedUnitSlot = null;
    }
}