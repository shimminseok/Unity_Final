using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SelectEquipUI : UIBase
{
    [Header("장비 슬롯")]
    [SerializeField] private List<InventorySlot> equippedItemsSlot = new();

    [Header("보유 장비 인벤토리 & 장착 버튼 프리팹")]
    [SerializeField] private Transform inventoryParent;

    [SerializeField] private EquipButton equipButtonPrefab;

    [Header("장비 정보 표시")]
    [SerializeField] private TMP_Text itemName;

    [SerializeField] private TMP_Text itemDescription;

    [Header("아바타 표시")]
    [SerializeField] private RawImage avatarImage;

    [Header("인벤토리")]
    [SerializeField] private EquipmentUnitInventoryUI inventoryUI;

    public EntryDeckData CurrentCharacter { get; private set; }

    public event Action<EntryDeckData> OnEquipChanged;


    private void Start()
    {
    }

    public override void Open()
    {
        base.Open();
        UpdateEquipUI();
        if (CurrentCharacter == null)
            return;
        AvatarPreviewManager.Instance.ShowAvatar(CurrentCharacter.CharacterSo, avatarImage);
    }

    public void SetCurrentSelectedUnit(EntryDeckData currentUnit)
    {
        DeckSelectManager.Instance.SetCurrentSelectedCharacter(currentUnit);
        CurrentCharacter = currentUnit;
    }

    public override void Close()
    {
        base.Close();
        AvatarPreviewManager.Instance.HideAllAvatars();
        OnEquipChanged?.Invoke(CurrentCharacter);
    }

    // UI 갱신
    public void UpdateEquipUI()
    {
        if (CurrentCharacter == null)
            return;

        ClearEquipInfo();
        equippedItemsSlot.ForEach(slot => slot.Initialize(null, false));
        foreach (var equipmentItem in CurrentCharacter.equippedItems)
        {
            equippedItemsSlot[(int)equipmentItem.Key].Initialize(equipmentItem.Value, false);
            equippedItemsSlot[(int)equipmentItem.Key].SetEquipMark(false);
        }

        inventoryUI.Initialize();
    }

    public void OnClickInventorySlot(EquipmentItem item)
    {
        if (item.IsEquipped && item.EquippedUnit != CurrentCharacter)
        {
            Debug.Log($"현재 장착된 아이템은 {item.EquippedUnit.CharacterSo.UnitName}이 장착하고 있습니다");
            return;
        }

        DeckSelectManager.Instance.SelectEquipment(item);
        UpdateEquipUI();
    }

    private EquipButton GetOrCreateEquipButton(int index, Transform parent, List<EquipButton> pool)
    {
        EquipButton btn;

        if (index < pool.Count)
        {
            btn = pool[index];
        }
        else
        {
            btn = Instantiate(equipButtonPrefab, parent);
            pool.Add(btn);
        }

        btn.transform.SetParent(parent, false);
        btn.gameObject.SetActive(true);
        return btn;
    }

    private void DisableRemainingButtons(int fromIndex, List<EquipButton> pool)
    {
        for (int i = fromIndex; i < pool.Count; i++)
        {
            pool[i].gameObject.SetActive(false);
        }
    }

    // 장비 정보 텍스트 표시
    private void ShowEquipInfo(EquipmentItem item)
    {
        itemName.text = item.EquipmentItemSo.ItemName;
        itemDescription.text = item.EquipmentItemSo.ItemDescription;
    }

    // 장비 정보 텍스트 삭제
    private void ClearEquipInfo()
    {
        itemName.text = "";
        itemDescription.text = "";
    }
}