using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [Header("장비 정보 표시")]
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemDescription;

    private EntryDeckData currentCharacter;



    public override void Open()
    {
        base.Open();
        UpdateEquipUI();
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
        DestroyChildren(weaponSlot);
        DestroyChildren(armorSlot);
        DestroyChildren(accessorySlot);
        DestroyChildren(inventoryParent);
    }



    // 보유 중인 장비 버튼 생성
    /// <summary>
    /// !!! 임시조치로 현재 테이블에 있는 모든 장비 가져옴!!!
    /// </summary>
    private void GenerateOwnedEquipButtons()
    {
        // 현재 선택한 캐릭터의 직업
        var job = currentCharacter.characterSO.JobType;

        // 테이블에서 장비 목록 가져오기
        var equipList = TableManager.Instance.GetTable<ItemTable>().GetEquipmentsByJob(job);

        foreach(var equipSO in equipList)
        {
            // SO 기반 임시 장비 인스턴스
            EquipmentItem equipItem = new EquipmentItem(equipSO);

            // 장착 여부를 SO 기준으로 판단
            bool isEquipped = currentCharacter.equippedItems.TryGetValue(
                equipSO.EquipmentType, out var equippedItem)
                && equipItem.EquipmentItemSo == equipSO;

            // 버튼 생성 및 초기화
            var btn = Instantiate(equipButtonPrefab, inventoryParent);
            btn.Initialize(equipItem, isEquipped, OnEquipButtonClicked);
        }
    }

    // 선택 중인 장비 슬롯 채우기
    private void GenerateSelectedEquipButton()
    {
        foreach (var item in currentCharacter.equippedItems)
        {
            if (item.Value == null) continue;

            var slot = GetSlotTransform(item.Key);
            if (slot == null) continue;

            var btn = Instantiate(equipButtonPrefab, slot);
            btn.Initialize(item.Value, true, OnEquipButtonClicked);
        }
    }



    // 클릭 콜백
    private void OnEquipButtonClicked(EquipButton btn, bool isEquipped)
    {
        ClearEquipInfo();

        var item = btn.GetEquipmentItem();
        var job = currentCharacter.characterSO.JobType;

        // 장착된 장비를 클릭하면 정보만 표시한다
        if (isEquipped)
        {
            ShowEquipInfo(item);
            return;
        }

        // 장착 제한 체크
        // !!!임시조치: 추후 장비를 다 띄워줄 수도 있기에!!!
        if(!item.EquipmentItemSo.IsEquipableByAllJobs &&
            item.EquipmentItemSo.JobType != job)
        {
            Debug.Log("이 직업은 이 장비를 장착할 수 없습니다.");
            return;
        }

        DeckSelectManager.Instance.SelectEquipment(item);
        UpdateEquipUI();
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
            EquipmentType.Weapon => weaponSlot,
            EquipmentType.Armor => armorSlot,
            EquipmentType.Accessory => accessorySlot,
            _ => null
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
