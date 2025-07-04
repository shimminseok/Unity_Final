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
        SetPlayerUnitData(new EntryDeckData() { characterSO = TableManager.Instance.GetTable<PlayerUnitTable>().GetDataByID(1) });
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
        SetPlayerUnitData(new EntryDeckData() { characterSO = TableManager.Instance.GetTable<PlayerUnitTable>().GetDataByID(1) });
    }

    public override void Close()
    {
    }
}