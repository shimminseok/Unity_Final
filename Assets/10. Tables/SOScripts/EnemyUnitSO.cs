using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyUnitSO", menuName = "ScriptableObjects/Unit/EnemyUnit", order = 0)]
public class EnemyUnitSO : UnitSO
{
    public AnimationClip MoveAniClip;
    public AnimationClip IdleAniClip;
    public AnimationClip DeadAniClip;
    public float skillActionProbability;
    public List<EnemySkillData> SkillDatas;
}

[System.Serializable]
public class EnemySkillData
{
    public ActiveSkillSO skillSO;
    public float individualProbability;
    
}