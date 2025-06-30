namespace PlayerState
{
    public class IdleState : IState<PlayerUnitController, PlayerUnitState>
    {
        private readonly int isMove = Define.MoveAnimationHash;

        public void OnEnter(PlayerUnitController owner)
        {
            owner.Agent.avoidancePriority = 1;
            owner.Animator.SetBool(isMove, false);
        }

        public void OnUpdate(PlayerUnitController owner)
        {
        }

        public void OnFixedUpdate(PlayerUnitController owner)
        {
        }

        public void OnExit(PlayerUnitController entity)
        {
        }
    }
}