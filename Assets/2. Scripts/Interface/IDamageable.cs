using System.Collections;
using UnityEngine;

public interface IDamageable
{
    public Collider    Collider       { get; }
    public BaseEmotion CurrentEmotion { get; }
    public bool        IsDead         { get; }
    public void        TakeDamage(float amount, StatModifierType modifierType = StatModifierType.Base);
    
    public StatusEffectManager StatusEffectManager { get; }

    public void ExecuteCoroutine(IEnumerator coroutine);

    public void ChangeEmotion(EmotionType newType);
    public void        Dead();
}