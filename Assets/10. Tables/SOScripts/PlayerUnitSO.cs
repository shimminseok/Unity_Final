using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewPlayerUnitSO", menuName = "ScriptableObject/Unit/PlayerUnit", order = 0)]
public class PlayerUnitSO : UnitSO, IStatProvider
{
    public PassiveSO PassiveSkill;

    //TODO : 스킬 리스트 
}