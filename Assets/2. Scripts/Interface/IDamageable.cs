using System;
using UnityEngine;

public interface IDamageable
{
    public bool     IsDead   { get; }
    public Collider Collider { get; }
    public void     TakeDamage(float amount, StatModifierType modifierType = StatModifierType.Base);
    public void     Dead();
}