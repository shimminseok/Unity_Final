using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PanelSelectedUnitInfo : MonoBehaviour
{
    [FormerlySerializedAs("inventoryItems")]
    [SerializeField] private InventorySlot[] unitEquippedItems = new InventorySlot[3];

    [SerializeField] private Image[] skillIcons = new Image[3];
    private EntryDeckData selectedUnitData;


    private SelectEquipUI SelectEquipUI => UIManager.Instance.GetUIComponent<SelectEquipUI>();
    private SelectSkillUI SelectSkillUI => UIManager.Instance.GetUIComponent<SelectSkillUI>();

    public void SetInfoPanel(EntryDeckData data)
    {
        data.OnEquipmmmentChanged -= UpdateEquippedItemSlot;
        this.selectedUnitData = data;
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

        data.OnEquipmmmentChanged += UpdateEquippedItemSlot;
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
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
}