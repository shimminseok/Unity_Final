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

    private void Start()
    {
        selectEquipUI = UIManager.Instance.GetUIComponent<SelectEquipUI>();
        selectSkillUI = UIManager.Instance.GetUIComponent<SelectSkillUI>();
    }

    private void Update()
    {
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
        foreach (EntryDeckData playerUnit in AccountManager.Instance.MyPlayerUnits.Values)
        {
            //Slot생성
            var slot = Instantiate(playerUnitSlot, playerUnitSlotRoot);
            slot.Initialize(playerUnit.CharacterSo, false, OnClickPlayerUnitSlot);
        }
    }

    public override void Close()
    {
        base.Close();
    }


    public void OpenSetEquipment()
    {
        selectEquipUI.SetCurrentSelectedUnit(SelectedPlayerUnitData);

        UIManager.Instance.Open<SelectEquipUI>();
    }

    public void OpenSetSkill()
    {
        selectSkillUI.SetCurrentSelectedUnit(SelectedPlayerUnitData);
        UIManager.Instance.Open<SelectSkillUI>();
    }
}