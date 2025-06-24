using UnityEngine;

public abstract class Unit : MonoBehaviour, IDamageable
{
    public StatManager         StatManager         { get; protected set; }
    public StatusEffectManager StatusEffectManager { get; protected set; }

    public          Collider      Collider      { get; protected set; }
    public          int           Index         { get; protected set; }
    public          bool          IsDead        { get; protected set; }
    public          bool          IsTurnEnded   { get; protected set; }
    protected       BattleManager BattleManager => BattleManager.Instance;
    public abstract void          StartTurn();

    public abstract void EndTurn();

    public abstract void TakeDamage(float amount, StatModifierType modifierType = StatModifierType.Base);

    public abstract void Dead();
}