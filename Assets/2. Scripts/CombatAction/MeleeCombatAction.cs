using System;
using System.Collections;
using UnityEngine;

public class MeleeCombatAction : ICombatAction
{
    public event Action OnActionComplete;

    public void Execute(Unit attacker)
    {
        TimeLineManager.Instance.PlayTimeLine(CameraManager.Instance.cinemachineBrain,CameraManager.Instance.skillCameraController,attacker);
        if (attacker is PlayerUnitController player)
        {
            if (player.CurrentAction == ActionType.Attack)
                player.ChangeUnitState(PlayerUnitState.Attack);
            else if (player.CurrentAction == ActionType.SKill)
                player.ChangeUnitState(PlayerUnitState.Skill);
        }
        else if (attacker is EnemyUnitController enemy)
        {
            if (enemy.CurrentAction == ActionType.Attack)
                enemy.ChangeUnitState(EnemyUnitState.Attack);
            else if (enemy.CurrentAction == ActionType.SKill)
                enemy.ChangeUnitState(EnemyUnitState.Skill);
        }

        attacker.ExecuteCoroutine(WaitForAnimationDone(attacker));
    }

    private IEnumerator WaitForAnimationDone(Unit attacker)
    {
        yield return new WaitUntil(() => attacker.IsAnimationDone || !attacker.IsTimeLinePlaying);
        OnActionComplete?.Invoke();
    }
}