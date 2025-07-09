using UnityEngine;

namespace EnemyState
{
    public class ReturnState : IState<EnemyUnitController, EnemyUnitState>
    {
        private readonly int isMove = Define.MoveAnimationHash;

        public void OnEnter(EnemyUnitController owner)
        {
            owner.Agent.speed = 10f;
            owner.Agent.avoidancePriority = 10;
            owner.setRemainDistance = 0.1f;
            owner.Animator.SetBool(isMove, true);
            owner.MoveTo(owner.StartPostion);
        }

        public void OnUpdate(EnemyUnitController owner)
        {
            if (owner.IsDead)
            {
                owner.ChangeUnitState(EnemyUnitState.Die);
            }
        }

        public void OnFixedUpdate(EnemyUnitController owner)
        {
        }

        public void OnExit(EnemyUnitController owner)
        {
            owner.Animator.SetBool(isMove, false);
            owner.Agent.updateRotation = false;
            owner.transform.localRotation = Quaternion.identity;
            owner.Agent.updateRotation = true;
            owner.Agent.isStopped = true;
            owner.Agent.velocity = Vector3.zero;
            owner.Agent.ResetPath();
        }
    }
}