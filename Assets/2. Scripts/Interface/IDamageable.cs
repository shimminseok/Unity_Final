using System;
using UnityEngine;

public interface IDamageable
{
    public Collider    Collider       { get; }
    public BaseEmotion CurrentEmotion { get; }
    public bool        IsDead         { get; }
    public void        TakeDamage(float amount, StatModifierType modifierType = StatModifierType.Base);

    public Action OnTakeDamageHandler { get; }
    public void        Dead();
}