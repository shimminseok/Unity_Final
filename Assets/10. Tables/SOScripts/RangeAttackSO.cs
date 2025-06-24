using UnityEngine;

[CreateAssetMenu(fileName = "NewRangeAttack", menuName = "ScriptableObject/AttackType/Range", order = 0)]
public class RangeAttackSO : AttackTypeSO
{
    public override void Attack()
    {
        Debug.Log("나는 원거리 공격이에오");
    }
}