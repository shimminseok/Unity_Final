using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoPanel : MonoBehaviour
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

    private void Awake()
    {
        // 편집 버튼에 클릭 이벤트 연결
        equipEditButton.onClick.AddListener(OnClickEquipUI);
        skillEditButton.onClick.AddListener(OnClickSkillUI);
    }

    public void SetData(EntryDeckData data)
    {
        if (data == null) return;

        // 이름 표시
        characterNameText.text = data.characterSO.UnitName;

        // 장비 표시
        weaponSlotImage.sprite = GetEquipSprite(data.equippedItems, EquipmentType.Weapon);
        armorSlotImage.sprite = GetEquipSprite(data.equippedItems, EquipmentType.Armor);
        accessorySlotImage.sprite = GetEquipSprite(data.equippedItems, EquipmentType.Accessory);

        // 액티브 스킬들 표시
        for (int i = 0; i < activeSkillSlotImage.Length; i++)
        {
            if (data.skillDatas[i] != null)
            {
                Debug.Log("액티브 스프라이트 할당됨");
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
            // !!!현재 패시브 스킬 아이콘이 없음!!!
            //passiveSkillSlotImage.sprite = data.passiveSkill.skillIcon;
            Debug.Log("패시브 스프라이트 할당됨");
        }

        else
        {
            passiveSkillSlotImage.sprite = null;
        }
    }

    // 장비 딕셔너리에서 원하는 장비 타입을 꺼내, 아이콘을 반환한다
    private Sprite GetEquipSprite(Dictionary<EquipmentType, EquipmentItem> dic, EquipmentType type)
    {
        if (dic.TryGetValue(type, out var item) && item != null)
        {
            return item.EquipmentItemSo.ItemSprite;
        }

        else
        {
            return null;
        }
    }

    // 장비 편집창 열기
    private void OnClickEquipUI()
    {
        var entry = DeckSelectManager.Instance.GetSelectedDeck();

        if(entry != null)
        {
            UIManager.Instance.Open<SelectEquipUI>();
        }
    }

    // 스킬 편집창 열기
    private void OnClickSkillUI()
    {
        var entry = DeckSelectManager.Instance.GetSelectedDeck();

        if (entry != null)
        {
            UIManager.Instance.Open<SelectSkillUI>();

        }

    }
}
