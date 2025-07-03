using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRangeSkillSO", menuName = "ScriptableObjects/SKillType/RangeWithoutProjectile", order = 0)]
public class RangeWithoutProjectileSO : RangeSkillSO
{
    public override void Execute(Unit attacker, IDamageable target)
    {
        attacker.SkillController.UseSkill();
    }
}