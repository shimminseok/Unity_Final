using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewPlayerUnitIncreaseSO", menuName = "ScriptableObjects/Increase/PlayerUnitIncrease", order = 0)]
public class PlayerUnitIncreaseSo : ScriptableObject, IIncreaseStat
{
    public List<StatData> IncreaseStats;
    public List<StatData> Stats => IncreaseStats;
}