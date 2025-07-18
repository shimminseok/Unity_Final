using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IReuseScrollData<InventoryItem>
{
    public int DataIndex { get; private set; }


    [SerializeField] private Image itemIcon;
    [SerializeField] private Image itemSlotFrame;
    [SerializeField] private Image itemEquipmentImg;
    [SerializeField] private List<GameObject> itemGradeStars;

    [SerializeField] private Sprite emptySlotSprite;
    [SerializeField] private List<Sprite> itemGradeSprites;

    [SerializeField] private TextMeshProUGUI amountTxt;

    public EquipmentItem Item { get; private set; }

    public event Action<EquipmentItem> OnClickSlot;

    private Action<InventorySlot> onClickCallback;

    private InventoryManager inventoryManager => InventoryManager.Instance;

    public void Initialize(EquipmentItem item, bool isHide)
    {
        if (item == null)
        {
            EmptySlot(isHide);
            return;
        }

        gameObject.SetActive(true);
        ShowEquipMark(item.IsEquipped);
        itemSlotFrame.sprite = itemGradeSprites[(int)item.ItemSo.Tier];

        itemIcon.gameObject.SetActive(true);
        itemIcon.sprite = item.ItemSo.ItemSprite;

        for (int i = 0; i < itemGradeStars.Count; i++)
        {
            itemGradeStars[i].SetActive(i <= (int)item.ItemSo.Tier);
        }

        amountTxt.text = item.Quantity > 0 ? $"x{item.Quantity}" : "";
        Item = item;
    }

    public void Initialize(RewardData rewardData)
    {
        EmptySlot(true);
        if (rewardData != null)
        {
            gameObject.SetActive(true);
            amountTxt.gameObject.SetActive(true);
            if (rewardData.RewardType != RewardType.Item)
            {
                amountTxt.text = $"x{rewardData.Amount}";
            }
        }
    }


    public void EmptySlot(bool isHide)
    {
        // 아이템 아이콘 비우기
        itemIcon.sprite = null;
        itemIcon.gameObject.SetActive(false);

        // 아이템 프레임 비활성화
        itemSlotFrame.sprite = emptySlotSprite;
        // 장비 이미지 비우기
        itemEquipmentImg.gameObject.SetActive(false);
        foreach (GameObject go in itemGradeStars)
        {
            go.SetActive(false);
        }

        Item = null;
        gameObject.SetActive(!isHide);
        amountTxt.gameObject.SetActive(false);
    }

    public void ShowEquipMark(bool isEquip)
    {
        itemEquipmentImg.gameObject.SetActive(isEquip);
    }

    public void OnClickSlotBtn()
    {
        OnClickSlot?.Invoke(Item);
    }

    public void SetOnClickCallback(Action<InventorySlot> callback)
    {
        onClickCallback = callback;
        onClickCallback?.Invoke(this);
    }

    public void UpdateSlot(ScrollData<InventoryItem> data)
    {
        DataIndex = data.DataIndex;
        Initialize(data.Data as EquipmentItem, isHide: false);
    }
}