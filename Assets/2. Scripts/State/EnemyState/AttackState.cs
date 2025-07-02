using UnityEngine;

namespace EnemyState
{
    public class AttackState : IState<EnemyUnitController, EnemyUnitState>
    {
        private readonly int attack = Define.AttackAnimationHash;

        public void OnEnter(EnemyUnitController owner)
        {
            owner.Agent.updateRotation = false;
            owner.transform.LookAt(owner.IsCounterAttack ? owner.CounterTarget.Collider.transform : owner.Target.Collider.transform);
            owner.Agent.isStopped = true;
            owner.Agent.velocity = Vector3.zero;
            owner.Agent.ResetPath();
            owner.Animator.SetTrigger(attack);
        }

        public void OnUpdate(EnemyUnitController owner)
        {
        }

        public void OnFixedUpdate(EnemyUnitController owner)
        {
        }

        public void OnExit(EnemyUnitController owner)
        {
            owner.Animator.ResetTrigger(attack);
            owner.Agent.isStopped = false;
            owner.Agent.updateRotation = true;
        }
    }
}