using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyUnitSO", menuName = "ScriptableObjects/Unit/EnemyUnit", order = 0)]
public class EnemyUnitSO : UnitSO
{
    public AnimationClip MoveAniClip;
    public AnimationClip IdleAniClip;
    public AnimationClip DeadAniClip;
}