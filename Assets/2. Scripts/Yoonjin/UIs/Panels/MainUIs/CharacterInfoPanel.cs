using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoPanel : UIBase
{
    [Header("편집 버튼")]
    public Button equipEditButton;
    public Button skillEditButton;

    [Header("캐릭터 이름")]
    [SerializeField] private TMP_Text characterNameText;

    [Header("장비 슬롯")]
    [SerializeField] private Image weaponSlotImage;
    [SerializeField] private Image armorSlotImage;
    [SerializeField] private Image accessorySlotImage;

    [Header("액티브 스킬 슬롯")]
    [SerializeField] private Image[] activeSkillSlotImage = new Image[3];

    [Header("패시브 스킬 슬롯")]
    [SerializeField] private Image passiveSkillSlotImage;

    public void SetData(EntryDeckData data)
    {
        if (data == null) return;

        // 이름 표시
        characterNameText.text = data.characterSO.UnitName;

        // 장비 표시
        weaponSlotImage.sprite = GetItemSprite(data.equippedItems, EquipmentType.Weapon);
        armorSlotImage.sprite = GetItemSprite(data.equippedItems, EquipmentType.Armor);
        accessorySlotImage.sprite = GetItemSprite(data.equippedItems, EquipmentType.Accessory);

        // 액티브 스킬들 표시
        for (int i = 0; i < activeSkillSlotImage.Length; i++)
        {
            if (data.skillDatas[i] != null)
            {
                activeSkillSlotImage[i].sprite = data.skillDatas[i].skillIcon;
            }

            else
            {
                activeSkillSlotImage[i].sprite = null;
            }
        }

        // 패시브 스킬 표시
        if (data.passiveSkill != null)
        {
            passiveSkillSlotImage.sprite = data.passiveSkill.skillIcon;
        }

        else
        {
            passiveSkillSlotImage.sprite = null;
        }
    }

    // 장비 딕셔너리에서 원하는 장비 타입을 꺼내, 아이콘을 반환한다
    private Sprite GetItemSprite(Dictionary<EquipmentType, EquipmentItemSO> dic, EquipmentType type)
    {
        if (dic.TryGetValue(type, out var item) && item != null)
        {
            return item.ItemSprite;
        }

        else
        {
            return null;
        }
    }
}
