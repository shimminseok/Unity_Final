using System;
using UnityEngine;

public interface IDamageable
{
    public int  Index  { get; }
    public bool IsDead { get; }
    public void TakeDamage(float amount, StatModifierType modifierType = StatModifierType.Base);
    public void Dead();
}