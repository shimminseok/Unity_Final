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
    Counter,
    Defense,

    Speed,

    CriticalDam,
    CriticalRate,
}

public enum StatModifierType
{
    Base,
    BuffFlat,
    BuffPercent,
    Equipment,

    BasePercent
}

public enum PlayerUnitState
{
    Idle,
    Move,
    Attack,
    Hit,
    Skill,
    Die,
    Stun,
}

public enum EnemyUnitState
{
    Idle,
    Move,
    Attack,
    Hit,
    Die
}

public enum StatusEffectType
{
    InstantBuff,          //즉발
    OverTimeBuff,         //시간
    InstantDebuff,        // 즉발 디버프
    OverTimeDebuff,       // 시간 디버프
    TimedModifierBuff,    //일정 시간동안 유지되는 (1턴으로 지정)
    PeriodicDamageDebuff, //도트뎀
    Recover,              //회복
    RecoverOverTime,      // 지속 시간 동안 회복
    Damege,               // 즉발 대미지
}

public enum Emotion
{
    Neutral,    //일반
    Anger,      //분노
    Depression, //우울
    Joy         // 기쁨
}

public enum ItemType
{
}

public enum EquipmentType
{
}

/*SelectTargetType : 스킬을 사용할 때 적을 선택하는 로직 타입
 * EnemySingle : 적 한명
 * EnemyTwoRandom : 적 한 명 선택, 한 명 랜덤
 * EnemyAll : 적 모두
 * PlayerSingle : 플레이어 한 명
 * PlayerTwoRandom : 플레이어 한 명 선택, 한 명 랜덤
 * PlayerAll : 플레이어 전체
 */
public enum SelectTargetType
{
    EnemySingle,
    //EnemyConShaped,
    EnemyTwoRandom,
    EnemyAll,
    PlayerSingle,
    PlayerTwoRandom,
    //PlayerConShaped,
    PlayerAll,
}

/*
 *  Male_Warrior : 남자기사
    FeMale_Warrior : 여자기사
    SpearMan : 창술사
    DragonKnight : 용기사
    Archer : 궁수
    Priest : 성직자
    Mage : 마법사
 *
 */

public enum JobType
{
    Male_Warrior,
    FeMale_Warrior,
    SpearMan,
    DragonKnight,
    Archer,
    Priest,
    Mage
}