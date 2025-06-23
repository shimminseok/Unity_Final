using UnityEngine;


[CreateAssetMenu(fileName = "NewMeleeAttack", menuName = "ScriptableObject/AttackType/Melee", order = 0)]
public class MeleeAttack : AttackType
{
    public override void Attack()
    {
        Debug.Log("근접 공격 이에요.");
    }
}