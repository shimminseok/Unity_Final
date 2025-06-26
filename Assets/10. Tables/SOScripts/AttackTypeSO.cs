using UnityEngine;


public abstract class AttackTypeSO : ScriptableObject, IAttackAction
{
    public abstract void               Attack(Unit attacker);
    public abstract AttackDistanceType DistanceType { get; }
}