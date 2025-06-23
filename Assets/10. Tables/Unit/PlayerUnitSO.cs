using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewPlayerUnitSO", menuName = "ScriptableObject/Unit/PlayerUnit", order = 0)]
public class PlayerUnitSO : ScriptableObject, IStatProvider
{
    public int ID;
    public string UnitName;

    public List<StatData> UnitStats;

    //TODO : 스킬 리스트
    public List<StatData> Stats => UnitStats;
}