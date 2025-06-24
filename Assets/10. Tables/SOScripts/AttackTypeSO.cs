using UnityEngine;


public abstract class AttackTypeSO : ScriptableObject
{
    public Unit Owner { get; private set; }

    public void Initialize(Unit owner)
    {
        Owner = owner;
    }

    public abstract void Attack();
}