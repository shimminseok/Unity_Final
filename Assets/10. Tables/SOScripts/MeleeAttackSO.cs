using UnityEngine;


[CreateAssetMenu(fileName = "NewMeleeAttack", menuName = "ScriptableObject/AttackType/Melee", order = 0)]
public class MeleeAttackSO : AttackTypeSO
{
    public override void Attack()
    {
        Debug.Log("근접 공격 이에요.");
    }
}