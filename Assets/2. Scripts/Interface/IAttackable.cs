using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    StatBase AttackStat { get; }

    public Collider    Collider       { get; }
    public IDamageable Target { get; }
    public void        Attack();

    public void SetTarget(Unit target);
}