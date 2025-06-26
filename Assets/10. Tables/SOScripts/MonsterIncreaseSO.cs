using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewMonsterIncreaseSO", menuName = "ScriptableObjects/MonsterIncrease/MonsterIncrease", order = 0)]
public class MonsterIncreaseSO : ScriptableObject
{
    public List<StatData> IncreaseStats;
}