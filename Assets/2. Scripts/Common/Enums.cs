using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    MaxHp,
    CurHp,

    MaxMp,
    CurMp,
    
    AttackPow,
    AttackRange,

    Defense,
    
}

public enum StatModifierType
{
    Base,
    BuffFlat,
    BuffPercent,
    Equipment,

    BasePercent
}