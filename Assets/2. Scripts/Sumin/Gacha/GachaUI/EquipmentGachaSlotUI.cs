using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class EquipmentGachaSlotUI : MonoBehaviour
{
    [SerializeField] private Image equipmentImage;
    [SerializeField] private TextMeshProUGUI equipmentNameText;
    [SerializeField] private Image itemSlotFrame;
    [SerializeField] private List<GameObject> itemGradeStars;
    [SerializeField] private List<Sprite> itemGradeSprites;

    public void Initialize(EquipmentItemSO equipment)
    {
        equipmentImage.sprite = equipment.ItemSprite;
        equipmentNameText.text = equipment.ItemName;
        itemSlotFrame.sprite = itemGradeSprites[(int)equipment.Tier];

        for (int i = 0; i < itemGradeStars.Count; i++)
        {
            itemGradeStars[i].SetActive(i <= (int)equipment.Tier);
        }
    }
}
