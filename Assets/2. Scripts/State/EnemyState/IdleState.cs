namespace EnemyState
{
    public class IdleState : IState<EnemyUnitController, EnemyUnitState>
    {
        public void OnEnter(EnemyUnitController owner)
        {
            owner.Agent.avoidancePriority = 1;
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
}