using UnityEngine;

namespace EnemyState
{
    public class IdleState : IState<EnemyUnitController, EnemyUnitState>
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

    public class MoveState : IState<EnemyUnitController, EnemyUnitState>
    {
        public void OnEnter(EnemyUnitController owner)
        {
            Debug.Log("Enter Move State");
            owner.MoveTo(owner.Target.Collider.transform.position);
        }

        public void OnUpdate(EnemyUnitController owner)
        {
            owner.MoveTo(owner.Target.Collider.transform.position);
        }

        public void OnFixedUpdate(EnemyUnitController owner)
        {
        }

        public void OnExit(EnemyUnitController entity)
        {
        }
    }

    public class AttackState : IState<EnemyUnitController, EnemyUnitState>
    {
        private static readonly int Attack = Animator.StringToHash("Attack");

        public void OnEnter(EnemyUnitController owner)
        {
            owner.Animator.SetTrigger(Attack);
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

    public class ReturnState : IState<EnemyUnitController, EnemyUnitState>
    {
        public void OnEnter(EnemyUnitController owner)
        {
        }

        public void OnUpdate(EnemyUnitController owner)
        {
            owner.MoveTo(owner.StartPostion);
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

        public EnemyUnitState CheckTransition(EnemyUnitController owner)
        {
            return EnemyUnitState.Stun;
        }
    }
}