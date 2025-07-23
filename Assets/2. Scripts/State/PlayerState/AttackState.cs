using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class AttackState : IState<PlayerUnitController, PlayerUnitState>
{
    private readonly int attack = Define.AttackAnimationHash;

    public void OnEnter(PlayerUnitController owner)
    {
        owner.OnToggleNavmeshAgent(true);
        owner.Agent.updateRotation = false;
        owner.transform.LookAt(owner.IsCounterAttack ? owner.CounterTarget.Collider.transform : owner.Target.Collider.transform);
        owner.Agent.isStopped = true;
        owner.Agent.velocity = Vector3.zero;
        owner.Agent.ResetPath();
        owner.Animator.SetTrigger(attack);
    }

    public void OnUpdate(PlayerUnitController owner)
    {
    }

    public void OnFixedUpdate(PlayerUnitController owner)
    {
    }

    public void OnExit(PlayerUnitController owner)
    {
        owner.Animator.ResetTrigger(attack);
        owner.Agent.isStopped = false;
        owner.Agent.updateRotation = true;
        owner.OnToggleNavmeshAgent(false);

    }
}