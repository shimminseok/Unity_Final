using System;
using System.Collections;
using UnityEngine;

public class MeleeCombatAction : ICombatAction
{
    public event Action OnActionComplete;

    public void Execute(Unit attacker)
    {
        if (attacker is PlayerUnitController player)
        {
            player.ChangeUnitState(PlayerUnitState.Attack);
        }
        else if (attacker is EnemyUnitController enemy)
        {
            enemy.ChangeUnitState(EnemyUnitState.Attack);
        }

        attacker.ExecuteCoroutine(WaitForAnimationDone(attacker));
    }

    private IEnumerator WaitForAnimationDone(Unit attacker)
    {
        yield return new WaitUntil(() => attacker.IsAnimationDone);
        OnActionComplete?.Invoke();
    }
}

public class RangeCombatAction : ICombatAction
{
    private readonly RangeActionSo attackData;
    private readonly IDamageable target;

    public event Action OnActionComplete;

    public RangeCombatAction(RangeActionSo so, IDamageable target)
    {
        attackData = so;
        this.target = target;
    }

    public void Execute(Unit attacker)
    {
        attacker.ExecuteCoroutine(WaitForSpawnProjectile());
    }

    private IEnumerator WaitForSpawnProjectile()
    {
        yield return new WaitUntil(() => attackData.ProjectileComponent != null);
        attackData.ProjectileComponent.trigger.OnTriggerTarget += () =>
        {
            OnActionComplete?.Invoke();
        };
    }
}