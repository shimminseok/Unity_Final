using System;
using UnityEngine;





[Serializable]
public class StatusEffectData
{
    public int ID;
    public StatusEffectType EffectType;
    public StatData Stat;
    public float Duration;
    public float TickInterval;
    public bool IsStackable;
}