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