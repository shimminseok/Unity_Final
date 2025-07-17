public class GachaUI : UIBase
{
    private SkillGachaUI skillGachaUI;
    private EquipmentGachaUI equipmentGachaUI;
    private CharacterGachaUI characterGachaUI;

    private UIManager uiManager;

    private void Start()
    {
        uiManager = UIManager.Instance;
        skillGachaUI = uiManager.GetUIComponent<SkillGachaUI>();
        equipmentGachaUI = uiManager.GetUIComponent<EquipmentGachaUI>();
        characterGachaUI = uiManager.GetComponent<CharacterGachaUI>();
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

    public void OnClickCharacterGachaBtn()
    {
        CloseAllGachaUI();
        uiManager.Open(characterGachaUI);
    }

    private void CloseAllGachaUI()
    {
        if (skillGachaUI == null)
            skillGachaUI = uiManager.GetUIComponent<SkillGachaUI>();

        if (equipmentGachaUI == null)
            equipmentGachaUI = uiManager.GetUIComponent<EquipmentGachaUI>();

        if (characterGachaUI == null)
            characterGachaUI = uiManager.GetUIComponent<CharacterGachaUI>();

        uiManager.Close(skillGachaUI);
        uiManager.Close(equipmentGachaUI);
        uiManager.Close(characterGachaUI);
    }
}