public class CharacterGachaHandler : IGachaHandler
{
    private readonly CharacterGachaSystem gachaSystem;
    private readonly CharacterGachaResultUI resultUI;
    private readonly UIManager uiManager;

    public CharacterGachaHandler(CharacterGachaSystem characterGachaSystem, CharacterGachaResultUI characterResultUI)
    {
        this.gachaSystem = characterGachaSystem;
        this.resultUI = characterResultUI;
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
        return "영웅 소환";
    }

    public void DrawAndDisplayResult(int count)
    {
        PlayerUnitSO[] characters = gachaSystem.DrawCharacters(count);

        uiManager.Open(resultUI);
        resultUI.ShowCharacters(characters);
    }

}
