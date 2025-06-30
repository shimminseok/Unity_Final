namespace PlayerState
{
    public class SkillState : IState<PlayerUnitController, PlayerUnitState>
    {
        private readonly int skill = Define.SkillAnimationHash;

        public void OnEnter(PlayerUnitController owner)
        {
            owner.Animator.SetTrigger(skill);
            owner.UseSkill();
        }

        public void OnUpdate(PlayerUnitController owner)
        {
        }

        public void OnFixedUpdate(PlayerUnitController owner)
        {
        }

        public void OnExit(PlayerUnitController owner)
        {
            owner.Animator.ResetTrigger(skill);
        }
    }
}