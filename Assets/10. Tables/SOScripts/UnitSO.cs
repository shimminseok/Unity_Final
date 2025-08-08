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

    [Header("무기로 때릴때의 보이스 사운드")]public SFXName AttackVoiceSound;
    [Header("데미지를 입었을 때의 보이스 사운드")]public SFXName HitVoiceSound;
    [Header("죽을 때의 효과음")]public SFXName DeadSound;

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