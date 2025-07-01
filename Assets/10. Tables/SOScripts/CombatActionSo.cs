using UnityEngine;


public abstract class CombatActionSo : ScriptableObject, IAttackAction
{
    public abstract void               Execute(Unit attacker);
    public abstract AttackDistanceType DistanceType { get; }
    public virtual  CombatActionSo     ActionSo     => this;
}