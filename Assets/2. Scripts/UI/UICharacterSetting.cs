using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICharacterSetting : UIBase
{
    [SerializeField] private Transform playerUnitSlotRoot;
    [SerializeField] private CharacterInfo characterInfoPanel;
    public EntryDeckData SelectedPlayerUnitData { get; private set; }


    private void Start()
    {
    }

    private void Update()
    {
    }

    public void SetPlayerUnitData(EntryDeckData playerUnitData)
    {
        SelectedPlayerUnitData = playerUnitData;
        characterInfoPanel.OpenPanel(playerUnitData);
    }

    public override void Open()
    {
        base.Open();
        foreach (EntryDeckData playerUnit in AccountManager.Instance.MyPlayerUnits.Values)
        {
            //Slot생성
        }
    }

    public override void Close()
    {
        base.Close();
    }
}