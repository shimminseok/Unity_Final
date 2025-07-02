namespace PlayerState
{
    public class MoveState : IState<PlayerUnitController, PlayerUnitState>
    {
        private readonly int isMove = Define.MoveAnimationHash;

        public void OnEnter(PlayerUnitController owner)
        {
            owner.Agent.avoidancePriority = 10;
            owner.Agent.isStopped = false;
            owner.setRemainDistance = 1.5f;
            owner.Animator.SetBool(isMove, true);
            owner.MoveTo(owner.Target.Collider.transform.position);
        }

        public void OnUpdate(PlayerUnitController owner)
        {
        }

        public void OnFixedUpdate(PlayerUnitController owner)
        {
        }

        public void OnExit(PlayerUnitController entity)
        {
            entity.Animator.SetBool(isMove, false);
        }
    }
}