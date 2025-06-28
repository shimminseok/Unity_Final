using UnityEngine;

namespace EnemyState
{
    public class IdleState : IState<EnemyUnitController, EnemyUnitState>
    {
        public void OnEnter(EnemyUnitController owner)
        {
            owner.Animator.SetBool(Define.MoveAnimationHash, false);
        }

        public void OnUpdate(EnemyUnitController owner)
        {
        }

        public void OnFixedUpdate(EnemyUnitController owner)
        {
        }

        public void OnExit(EnemyUnitController entity)
        {
        }
    }

    public class MoveState : IState<EnemyUnitController, EnemyUnitState>
    {
        private readonly int isMove = Define.MoveAnimationHash;

        public void OnEnter(EnemyUnitController owner)
        {
            owner.setRemainDistance = 1.5f;
            owner.Animator.SetBool(isMove, true);
            owner.MoveTo(owner.Target.Collider.transform.position);
        }

        public void OnUpdate(EnemyUnitController owner)
        {
            owner.MoveTo(owner.Target.Collider.transform.position);
        }

        public void OnFixedUpdate(EnemyUnitController owner)
        {
        }

        public void OnExit(EnemyUnitController owner)
        {
            owner.Animator.SetBool(isMove, false);
        }
    }

    public class ReturnState : IState<EnemyUnitController, EnemyUnitState>
    {
        private readonly int isMove = Define.MoveAnimationHash;

        public void OnEnter(EnemyUnitController owner)
        {
            owner.setRemainDistance = 0.1f;
            owner.transform.LookAt(owner.StartPostion, Vector3.up);
            owner.Animator.SetBool(isMove, true);
        }

        public void OnUpdate(EnemyUnitController owner)
        {
            owner.MoveTo(owner.StartPostion);
        }

        public void OnFixedUpdate(EnemyUnitController owner)
        {
        }

        public void OnExit(EnemyUnitController owner)
        {
            owner.Animator.SetBool(isMove, false);
        }
    }

    public class AttackState : IState<EnemyUnitController, EnemyUnitState>
    {
        private readonly int attack = Define.AttackAnimationHash;

        public void OnEnter(EnemyUnitController owner)
        {
            owner.Animator.SetTrigger(attack);
            owner.Attack();
        }

        public void OnUpdate(EnemyUnitController owner)
        {
        }

        public void OnFixedUpdate(EnemyUnitController owner)
        {
        }

        public void OnExit(EnemyUnitController entity)
        {
        }
    }

    public class DeadState : IState<EnemyUnitController, EnemyUnitState>
    {
        public void OnEnter(EnemyUnitController owner)
        {
        }

        public void OnUpdate(EnemyUnitController owner)
        {
        }

        public void OnFixedUpdate(EnemyUnitController owner)
        {
        }

        public void OnExit(EnemyUnitController entity)
        {
        }
    }

    public class StunState : IState<EnemyUnitController, EnemyUnitState>
    {
        public void OnEnter(EnemyUnitController owner)
        {
        }

        public void OnUpdate(EnemyUnitController owner)
        {
        }

        public void OnFixedUpdate(EnemyUnitController owner)
        {
        }

        public void OnExit(EnemyUnitController entity)
        {
        }
    }
}