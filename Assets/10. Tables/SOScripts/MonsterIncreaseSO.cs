using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewMonsterIncreaseSO", menuName = "ScriptableObjects/Increase/MonsterIncrease", order = 0)]
public class MonsterIncreaseSO : ScriptableObject, IIncreaseStat
{
    public List<StatData> IncreaseStats;
    public List<StatData> Stats => IncreaseStats;
}