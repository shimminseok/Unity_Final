public class EquipmentGachaHandler : IGachaHandler
{
    private readonly EquipmentGachaSystem gachaSystem;
    private readonly EquipmentGachaResultUI resultUI;
    private readonly UIManager uiManager;

    public EquipmentGachaHandler(EquipmentGachaSystem equipmentGachaSystem, EquipmentGachaResultUI equipmentResultUI)
    {
        this.gachaSystem = equipmentGachaSystem;
        this.resultUI = equipmentResultUI;
        uiManager = UIManager.Instance;
    }

    public bool CanDraw(int count)
    {
        return gachaSystem.CheckCanDraw(count);
    }

    public int GetDrawCost()
    {
        return gachaSystem.DrawCost;
    }

    public string GetGachaTypeName()
    {
        return "장비 소환";
    }

    public void DrawAndDisplayResult(int count)
    {
        EquipmentItemSO[] equipments = gachaSystem.DrawEquipments(count);

        uiManager.Open(uiManager.GetUIComponent<EquipmentGachaResultUI>());
        resultUI.ShowEquipments(equipments);
    }
}
