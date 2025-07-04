public class PlayerUnitData
{
    public int UnitSoId;
    public int Level;
    public int Amount;
    public int TranscendLevel;


    private const int BaseMaxLevel = 10;
    private const int MaxTranscendLevel = 5;

    public int MaxLevel => BaseMaxLevel + (TranscendLevel * BaseMaxLevel);

    public PlayerUnitData(int unitSoId)
    {
        UnitSoId = unitSoId;
        Level = 1;
        Amount = 1;
        TranscendLevel = 0;
    }

    public void LevelUp(out bool canLevelUp)
    {
        if (Level >= MaxLevel)
        {
            canLevelUp = false;
        }
        else
        {
            Level++;
            canLevelUp = true;
        }
    }

    public void Transcend()
    {
        TranscendLevel++;
    }
}