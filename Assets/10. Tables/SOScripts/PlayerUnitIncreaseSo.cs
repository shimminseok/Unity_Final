using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewPlayerUnitIncreaseSO", menuName = "ScriptableObjects/Increase/PlayerUnitIncrease", order = 0)]
public class PlayerUnitIncreaseSo : ScriptableObject, IIncreaseStat
{
    public List<StatData> IncreaseStats;
    public List<StatData> Stats => IncreaseStats;
    public float GetWeight(Tier tier)
    {
        switch (tier)
        {
            case Tier.A : return 0f;
            case Tier.S : return 1.15f;
            case Tier.SR : return 1.3f;
            case Tier.SSR : return 1.4f;
            default: return 0f;
        }
    }
}