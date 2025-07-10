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

    private EntryDeckData selectedUnitData;

    private void Awake()
    {
        // 편집 버튼에 클릭 이벤트 연결
        equipEditButton.onClick.AddListener(OnClickEquipUI);
        skillEditButton.onClick.AddListener(OnClickSkillUI);
    }

    private void UpdateEquipment(EntryDeckData data)
    {
        // 장비 표시
        weaponSlotImage.sprite = GetEquipSprite(data.equippedItems, EquipmentType.Weapon);
        armorSlotImage.sprite = GetEquipSprite(data.equippedItems, EquipmentType.Armor);
        accessorySlotImage.sprite = GetEquipSprite(data.equippedItems, EquipmentType.Accessory);
    }

    private void UpdateEquipmentSkill(EntryDeckData data)
    {
        // 액티브 스킬들 표시
        if (data == null)
            return;
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
    }

    public void SetData(EntryDeckData data)
    {
        if (data == null) return;

        // 이름 표시
        selectedUnitData = data;
        characterNameText.text = data.CharacterSo.UnitName;

        UpdateEquipment(selectedUnitData);
        UpdateEquipmentSkill(selectedUnitData);
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

        if (entry != null)
        {
            SelectEquipUI ui = UIManager.Instance.GetUIComponent<SelectEquipUI>();
            ui.OnEquipChanged -= UpdateEquipment;
            ui.SetCurrentSelectedUnit(selectedUnitData);
            UIManager.Instance.Open(ui);
            ui.OnEquipChanged += UpdateEquipment;
        }
    }

    // 스킬 편집창 열기
    private void OnClickSkillUI()
    {
        var entry = DeckSelectManager.Instance.GetSelectedDeck();

        if (entry != null)
        {
            SelectSkillUI ui = UIManager.Instance.GetUIComponent<SelectSkillUI>();
            ui.OnSkillChanged -= UpdateEquipmentSkill;
            ui.SetCurrentSelectedUnit(selectedUnitData);
            UIManager.Instance.Open(ui);
            ui.OnSkillChanged += UpdateEquipmentSkill;
        }
    }
}