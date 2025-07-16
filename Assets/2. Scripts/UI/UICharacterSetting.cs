using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICharacterSetting : UIBase
{
    [SerializeField] private Transform playerUnitSlotRoot;
    [SerializeField] private CharacterInfo characterInfoPanel;
    [SerializeField] private CharacterButton playerUnitSlot;


    public EntryDeckData SelectedPlayerUnitData { get; private set; }


    private SelectEquipUI selectEquipUI;
    private SelectSkillUI selectSkillUI;

    private Dictionary<int, CharacterButton> slotDic = new();


    private void Start()
    {
        selectEquipUI = UIManager.Instance.GetUIComponent<SelectEquipUI>();
        selectSkillUI = UIManager.Instance.GetUIComponent<SelectSkillUI>();
    }

    public void SetPlayerUnitData(EntryDeckData playerUnitData)
    {
        characterInfoPanel.OpenPanel(playerUnitData);
    }

    private void OnClickPlayerUnitSlot(int id, bool isSelectedSlot)
    {
        SelectedPlayerUnitData = AccountManager.Instance.GetPlayerUnit(id);

        if (SelectedPlayerUnitData == null)
            return;
        SetPlayerUnitData(SelectedPlayerUnitData);
    }

    public override void Open()
    {
        base.Open();

        var units = AccountManager.Instance.MyPlayerUnits;

        foreach (KeyValuePair<int, EntryDeckData> entryDeckData in units)
        {
            if (slotDic.ContainsKey(entryDeckData.Key))
                continue;

            var slot = Instantiate(playerUnitSlot, playerUnitSlotRoot);
            slot.Initialize(entryDeckData.Value.CharacterSo, true, OnClickPlayerUnitSlot);
            slotDic.Add(entryDeckData.Key, slot);
        }
    }

    public override void Close()
    {
        base.Close();
        characterInfoPanel.ClosePanel();
    }


    public void OpenSetEquipment()
    {
        selectEquipUI.SetCurrentSelectedUnit(SelectedPlayerUnitData);
        UIManager.Instance.Open(selectEquipUI);
    }

    public void OpenSetSkill()
    {
        selectSkillUI.SetCurrentSelectedUnit(SelectedPlayerUnitData);
        UIManager.Instance.Open(selectSkillUI);
    }
}