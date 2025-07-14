using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GachaUI : UIBase
{
    private SkillGachaUI skillGachaUI;
    private EquipmentGachaUI equipmentGachaUI;

    private UIManager uiManager;

    private void Start()
    {
        uiManager = UIManager.Instance;
        skillGachaUI = uiManager.GetUIComponent<SkillGachaUI>();
        equipmentGachaUI = uiManager.GetUIComponent<EquipmentGachaUI>();
    }

    public void OnClickSkillGachaBtn()
    {
        CloseAllGachaUI();
        uiManager.Open(skillGachaUI);
    }

    public void OnClickEquipmentGachaBtn()
    {
        CloseAllGachaUI();
        uiManager.Open(equipmentGachaUI);
    }

    private void CloseAllGachaUI()
    {
        uiManager.Close(skillGachaUI);
        uiManager.Close(equipmentGachaUI);
    }
}
