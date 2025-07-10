using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentGachaResultUI : UIBase
{
    [SerializeField] private Button resultExitBtn;
    [SerializeField] private EquipmentGachaSlotUI[] slots;

    void Start()
    {
        resultExitBtn.onClick.RemoveAllListeners();
        resultExitBtn.onClick.AddListener(() => OnResultPanelExitBtn());
    }

    public void ShowEquipments(EquipmentItemSO[] equipments)
    {
        for (int i=0; i<equipments.Length; i++)
        {
            slots[i].gameObject.SetActive(true);
            slots[i].Initialize(equipments[i]);
        }
    }

    public void OnResultPanelExitBtn()
    {
        for (int i=0; i<slots.Length; i++)
        {
            slots[i].gameObject.SetActive(false);
        }
        UIManager.Instance.Close(this);
    }
}
