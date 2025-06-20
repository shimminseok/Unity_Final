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

public enum PlayerState
{
    Idle,
    Move,
    Attack
}

public enum StatusEffectType
{
    InstantBuff,          //즉발
    OverTimeBuff,         //시간
    InstantDebuff,        // 즉발 디버프
    OverTimeDebuff,       // 시간 디버프
    TimedModifierBuff,    //일정 시간동안 유지되는
    PeriodicDamageDebuff, //도트뎀
    Recover,              //회복
    RecoverOverTime,      // 지속 시간 동안 회복
    Damege,               // 즉발 대미지
}