using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentGachaSlotUI : MonoBehaviour
{
    [SerializeField] private Image equipmentImage;
    [SerializeField] private TextMeshProUGUI equipmentNameText;
    [SerializeField] private TextMeshProUGUI equipmentTierText; // 나중에 프레임으로 변경

    public void Initialize(EquipmentItemSO equipment)
    {
        equipmentImage.sprite = equipment.ItemSprite;
        equipmentNameText.text = equipment.ItemName;
        equipmentTierText.text = $"{equipment.Tier}";
    }
}
