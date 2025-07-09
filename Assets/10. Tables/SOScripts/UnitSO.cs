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

    private Dictionary<StatType, StatData> statDic = new Dictionary<StatType, StatData>();

    public StatData GetStat(StatType statType)
    {
        if (!statDic.ContainsKey(statType))
        {
            statDic[statType] = UnitStats.Find(x => x.StatType == statType);
        }

        return statDic[statType];
    }
}