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

        public EnemyUnitState CheckTransition(EnemyUnitController owner)
        {
            return EnemyUnitState.Idle;
        }
    }

    public class MoveState : IState<EnemyUnitController, EnemyUnitState>
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
            return EnemyUnitState.Idle;
        }
    }

    public class AttackState : IState<EnemyUnitController, EnemyUnitState>
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
            return EnemyUnitState.Idle;
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

        public EnemyUnitState CheckTransition(EnemyUnitController owner)
        {
            return EnemyUnitState.Idle;
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