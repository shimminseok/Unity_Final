using UnityEngine;


public abstract class AttackTypeSO : ScriptableObject
{
    public abstract void Attack(Unit attacker);
}