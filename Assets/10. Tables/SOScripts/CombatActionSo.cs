using UnityEngine;


public abstract class CombatActionSo : ScriptableObject, IAttackAction
{
    public abstract void               Execute(IAttackable attacker, IDamageable target);
    public abstract AttackDistanceType DistanceType { get; }
    public virtual  CombatActionSo     ActionSo     => this;
}