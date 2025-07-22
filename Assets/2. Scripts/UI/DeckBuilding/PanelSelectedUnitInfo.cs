using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PanelSelectedUnitInfo : MonoBehaviour
{
    [FormerlySerializedAs("inventoryItems")]
    [SerializeField] private InventorySlot[] unitEquippedItems = new InventorySlot[3];

    [SerializeField] private SkillSlot passiveSkillSlot;
    [SerializeField] private SkillSlot[] activeSkillSlots = new SkillSlot[3];

    [SerializeField] private TextMeshProUGUI title;
    private EntryDeckData selectedUnitData;


    private SelectEquipUI SelectEquipUI => UIManager.Instance.GetUIComponent<SelectEquipUI>();
    private SelectSkillUI SelectSkillUI => UIManager.Instance.GetUIComponent<SelectSkillUI>();

    public void SetInfoPanel(EntryDeckData data)
    {
        data.OnEquipmmmentChanged -= UpdateEquippedItemSlot;
        data.OnSkillChanged -= UpdateEquippedSkillSlot;
        this.selectedUnitData = data;
        title.text = data.CharacterSo.UnitName;
        UpdateEquippedItemSlot();
        UpdateEquippedSkillSlot();
        data.OnEquipmmmentChanged += UpdateEquippedItemSlot;
        data.OnSkillChanged += UpdateEquippedSkillSlot;
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
        passiveSkillSlot.SetSkillIcon(selectedUnitData.CharacterSo.PassiveSkill, false);
        passiveSkillSlot.ShowEquipMark(false);
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }


    public void OnClickEditEquippedItemBtn()
    {
        SelectEquipUI.SetCurrentSelectedUnit(selectedUnitData);
        UIManager.Instance.Open(SelectEquipUI);
    }

    public void OnClickEditSkillBtn()
    {
        SelectSkillUI.SetCurrentSelectedUnit(selectedUnitData);
        UIManager.Instance.Open(SelectSkillUI);
    }

    private void UpdateEquippedItemSlot()
    {
        var equipItem = selectedUnitData.EquippedItems;
        for (int i = 0; i < unitEquippedItems.Length; i++)
        {
            if (equipItem.TryGetValue((EquipmentType)i, out EquipmentItem item))
            {
                unitEquippedItems[i].Initialize(item, false);
            }
            else
            {
                unitEquippedItems[i].Initialize(null, false);
            }

            unitEquippedItems[i].ShowEquipMark(false);
        }
    }

    private void UpdateEquippedSkillSlot()
    {
        SkillData[] equipSkill = selectedUnitData.SkillDatas;
        for (int i = 0; i < equipSkill.Length; i++)
        {
            SkillData skill = equipSkill[i];
            activeSkillSlots[i].SetSkillIcon(skill, false);
            activeSkillSlots[i].ShowEquipMark(false);
        }
    }
}