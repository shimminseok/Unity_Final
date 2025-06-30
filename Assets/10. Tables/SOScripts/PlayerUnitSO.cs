using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewPlayerUnitSO", menuName = "ScriptableObjects/Unit/PlayerUnit", order = 0)]
public class PlayerUnitSO : UnitSO
{
    public PassiveSO PassiveSkill;
    public JobType JobType;


    //TODO : 스킬 리스트 
}