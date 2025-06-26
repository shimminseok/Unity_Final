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

    HitRate,
    Shield
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

    EndTurn,
}

public enum EnemyUnitState
{
    Idle,
    Move,
    Attack,
    Hit,
    Stun,
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
    TurnBasedModifierBuff,
    Trigger,
}

public enum EmotionType
{
    None,       //없음
    Neutral,    // 노말
    Anger,      //분노
    Depression, //우울
    Joy         // 기쁨
}

public enum TriggerEventType
{
    OnAttacked, //피격 당했을때
}

public enum ItemType
{
    Equipment
}

public enum EquipmentType
{
    Weapon,
    Armor,
    Accessory
}

/*SelectTargetType : 스킬을 사용할 때 적을 선택하는 로직 타입
 * Single : 단일 타겟
 * All : 진형 전체
 * SinglePlusRandomOne : 진형 한 쪽의 단일 한 명과 랜덤 한 명
 */
public enum SelectTargetType
{
    Single,
    All,
    SinglePlusRandomOne,
}

/* 선택 가능한 진영
 * Player : Player쪽
 * Enemy : Enemy쪽
 * BothSide : 양 쪽 다
 */
public enum SelectCampType
{
    Player,
    Enemy,
    BothSide
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

public enum EquipJobRestrictionType
{
    All,
    Specific
}