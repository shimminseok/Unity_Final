using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterSetting : UIBase
{
    [SerializeField] private Transform playerUnitSlotRoot;
    [SerializeField] private CharacterInfo characterInfoPanel;
    [SerializeField] private UnitSlot playerUnitSlot;


    [Header("유닛 스탠딩 이미지")]
    [SerializeField] private CanvasGroup standingPanel;
    [SerializeField] private Image standingImage;
    [SerializeField] private float fadeInDuration;

    public EntryDeckData SelectedPlayerUnitData { get; private set; }


    private SelectEquipUI selectEquipUI;
    private SelectSkillUI selectSkillUI;

    private Dictionary<int, UnitSlot> slotDic = new();


    private void Start()
    {
        selectEquipUI = UIManager.Instance.GetUIComponent<SelectEquipUI>();
        selectSkillUI = UIManager.Instance.GetUIComponent<SelectSkillUI>();
    }

    public void SetPlayerUnitData(EntryDeckData playerUnitData)
    {
        characterInfoPanel.OpenPanel(playerUnitData);
    }

    private void OnClickPlayerUnitSlot(EntryDeckData playerUnitData)
    {
        SelectedPlayerUnitData = playerUnitData;

        if (SelectedPlayerUnitData == null)
            return;
        SetPlayerUnitData(SelectedPlayerUnitData);
        standingImage.sprite = SelectedPlayerUnitData.CharacterSo.UnitStanding;

        standingPanel.gameObject.SetActive(true);
        standingPanel.alpha = 0;
        standingPanel.DOFade(1f, fadeInDuration).SetEase(Ease.InOutSine);
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
            slot.name = $"UnitSlot_{entryDeckData.Value.CharacterSo.ID}";
            slot.Initialize(entryDeckData.Value);
            slotDic.Add(entryDeckData.Key, slot);
            slot.OnClicked += OnClickPlayerUnitSlot;
        }
    }

    public override void Close()
    {
        base.Close();
        characterInfoPanel.ClosePanel();
        standingPanel.gameObject.SetActive(false);
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