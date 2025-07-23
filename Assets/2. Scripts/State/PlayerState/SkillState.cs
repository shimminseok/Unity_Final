using UnityEngine;

namespace PlayerState
{
    public class SkillState : IState<PlayerUnitController, PlayerUnitState>
    {
        private readonly int skill = Define.SkillAnimationHash;

        public void OnEnter(PlayerUnitController owner)
        {
            owner.OnToggleNavmeshAgent(false);
            owner.Animator.SetTrigger(Define.SkillAnimationHash);
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
            owner.OnToggleNavmeshAgent(true);

        }
    }
}