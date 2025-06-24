using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStageSO", menuName = "ScriptableObject/Stage/StageSO", order = 0)]
public class StageSO : ScriptableObject
{
    public int ID;
    public int MonsterLevel;
    public List<EnemyUnitSO> Monsters;
    public MonsterIncreaseSO MonsterIncrease;
}