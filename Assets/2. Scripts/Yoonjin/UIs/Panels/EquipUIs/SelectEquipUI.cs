using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectEquipUI : UIBase
{
    [Header("장비 슬롯")]
    [SerializeField] private Transform weaponSlot;
    [SerializeField] private Transform armorSlot;
    [SerializeField] private Transform accessorySlot;

    [Header("보유 장비 인벤토리 & 장착 버튼 프리팹")]
    [SerializeField] private Transform inventoryParent;
    [SerializeField] private EquipButton equipButtonPrefab;

    private EntryDeckData currentCharacter;

    public override void Open()
    {
        base.Open();
    }

    // UI 갱신
    public void UpdateEquipUI()
    {
        currentCharacter = DeckSelectManager.Instance.GetCurrentSelectedCharacter();

        if (currentCharacter == null) return;

        
    }

    // 모두 삭제
    private void ClearAllSlots()
    {

    }

    // 보유 중인 장비 버튼 생성
    private void GenerateOwnedEquipButtons()
    {

    }
}
