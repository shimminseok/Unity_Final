using System.Collections.Generic;
using UnityEngine;


public class UnitSO : ScriptableObject, IStatProvider
{
    public int ID;
    public string UnitName;
    public List<StatData> UnitStats;
    public List<StatData> Stats => UnitStats;
    public CombatActionSo AttackType;

    public GameObject UnitPrefab;
    public Sprite UnitIcon;

    public AnimationClip AttackAniClip;
}