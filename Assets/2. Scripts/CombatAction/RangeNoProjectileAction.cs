using System;
using System.Collections;
using UnityEngine;

public class RangeNoProjectileAction : ICombatAction
{
    private readonly RangeActionSo attackData;
    private readonly IDamageable target;

    public event Action OnActionComplete;


    public void Execute(Unit attacker)
    {
        attacker.ExecuteCoroutine(WaitForAnimationDone(attacker));
    }

    private IEnumerator WaitForAnimationDone(Unit attacker)
    {
        yield return new WaitUntil(() => attacker.IsAnimationDone);
        OnActionComplete?.Invoke();
    }
}
