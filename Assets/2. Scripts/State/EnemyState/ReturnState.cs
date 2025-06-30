namespace EnemyState
{
    public class ReturnState : IState<EnemyUnitController, EnemyUnitState>
    {
        private readonly int isMove = Define.MoveAnimationHash;

        public void OnEnter(EnemyUnitController owner)
        {
            owner.Agent.avoidancePriority = 10;
            owner.setRemainDistance = 0.1f;
            owner.Animator.SetBool(isMove, true);
            owner.MoveTo(owner.StartPostion);
        }

        public void OnUpdate(EnemyUnitController owner)
        {
        }

        public void OnFixedUpdate(EnemyUnitController owner)
        {
        }

        public void OnExit(EnemyUnitController owner)
        {
            owner.Animator.SetBool(isMove, false);
        }
    }
}