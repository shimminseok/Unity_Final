using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EquipButton : MonoBehaviour
{
    [Header("이미지 / 타입")]
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text typeText;
    [SerializeField] private Button button;

    private EquipmentItem equip;
    private Action<EquipButton, bool> onClick;
    private bool isEquipped;

    public void Initialize(EquipmentItem item, bool isEquipped, Action<EquipButton, bool> callback)
    {
        equip = item;
        this.isEquipped = isEquipped;
        onClick = callback;

        icon.sprite = item.EquipmentItemSo.ItemSprite;
        typeText.text = item.EquipmentItemSo.EquipmentType.ToString();

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        onClick?.Invoke(this, isEquipped);
    }

    #region
    public EquipmentItem GetEquipmentItem() => equip;
    public bool IsEquipped => isEquipped;
    #endregion
}
