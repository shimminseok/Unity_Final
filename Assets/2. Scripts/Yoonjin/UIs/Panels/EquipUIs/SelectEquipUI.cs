using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectEquipUI : UIBase
{
    [Header("장비 슬롯")]
    [SerializeField] private Transform weaponSlot;

    [SerializeField] private Transform armorSlot;
    [SerializeField] private Transform accessorySlot;

    [Header("보유 장비 인벤토리 & 장착 버튼 프리팹")]
    [SerializeField] private Transform inventoryParent;

    [SerializeField] private EquipButton equipButtonPrefab;

    [Header("장비 정보 표시")]
    [SerializeField] private TMP_Text itemName;

    [SerializeField] private TMP_Text itemDescription;

    [Header("아바타 표시")]
    [SerializeField] private RawImage avatarImage;

    private EntryDeckData currentCharacter;
    private List<EquipButton> ownedEquipButtonPool = new();
    private List<EquipButton> selectedEquipButtonPool = new();
    public event Action<EntryDeckData> OnEquipChanged;

    public override void Open()
    {
        base.Open();
        UpdateEquipUI();
        if (currentCharacter == null)
            return;
        AvatarPreviewManager.Instance.ShowAvatar(currentCharacter.CharacterSo, avatarImage);
    }

    public void SetCurrentSelectedUnit(EntryDeckData currentUnit)
    {
        DeckSelectManager.Instance.SetCurrentSelectedCharacter(currentUnit);
        currentCharacter = currentUnit;
    }

    public override void Close()
    {
        base.Close();
        AvatarPreviewManager.Instance.HideAllAvatars();
        OnEquipChanged?.Invoke(currentCharacter);
    }

    // UI 갱신
    public void UpdateEquipUI()
    {
        if (currentCharacter == null)
            return;

        ClearEquipInfo();
        GenerateOwnedEquipButtons();
        GenerateSelectedEquipButtons();
    }


    private void GenerateOwnedEquipButtons()
    {
        int poolIndex = 0;
        var job       = currentCharacter.CharacterSo.JobType;

        var equipList = TableManager.Instance.GetTable<ItemTable>().GetEquipmentsByJob(job);

        foreach (var equipSO in equipList)
        {
            EquipmentType type = equipSO.EquipmentType;
            EquipmentItem equipItem;
            bool          isEquipped = false;

            if (currentCharacter.equippedItems.TryGetValue(type, out var equippedItem) &&
                equippedItem.EquipmentItemSo == equipSO)
            {
                equipItem = equippedItem;
                isEquipped = true;
            }
            else
            {
                equipItem = new EquipmentItem(equipSO);
            }

            var btn = GetOrCreateEquipButton(poolIndex++, inventoryParent, ownedEquipButtonPool);
            btn.Initialize(equipItem, isEquipped, false, OnEquipButtonClicked);
        }

        DisableRemainingButtons(poolIndex, ownedEquipButtonPool);
    }

    // 선택 중인 장비 슬롯 채우기
    private void GenerateSelectedEquipButtons()
    {
        int poolIndex = 0;

        foreach (var kvp in currentCharacter.equippedItems)
        {
            if (kvp.Value == null) continue;

            var slot = GetSlotTransform(kvp.Key);
            if (slot == null) continue;

            var btn = GetOrCreateEquipButton(poolIndex++, slot, selectedEquipButtonPool);
            btn.Initialize(kvp.Value, true, true, OnEquipButtonClicked);
        }

        DisableRemainingButtons(poolIndex, selectedEquipButtonPool);
    }

    // 클릭 콜백
    private void OnEquipButtonClicked(EquipButton btn, bool isEquipped)
    {
        ClearEquipInfo();

        var item = btn.GetEquipmentItem();
        var job  = currentCharacter.CharacterSo.JobType;

        // 장착된 장비를 클릭하면 정보만 표시한다
        if (btn.IsSlotButton)
        {
            ShowEquipInfo(item);
            return;
        }

        // 장착 제한 체크
        // !!!임시조치: 추후 장비를 다 띄워줄 수도 있기에!!!
        if (!item.EquipmentItemSo.IsEquipableByAllJobs &&
            item.EquipmentItemSo.JobType != job)
        {
            Debug.Log("이 직업은 이 장비를 장착할 수 없습니다.");
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


    // 슬롯 위치(장비 타입) 받아오기
    private Transform GetSlotTransform(EquipmentType type)
    {
        return type switch
        {
            EquipmentType.Weapon    => weaponSlot,
            EquipmentType.Armor     => armorSlot,
            EquipmentType.Accessory => accessorySlot,
            _                       => null
        };
    }

    // 초기화: 자식 오브젝트 삭제 (EquipInfo 진입할 때)
    private void DestroyChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }
}