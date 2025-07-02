using UnityEngine;

namespace PlayerState
{
    public class SkillState : IState<PlayerUnitController, PlayerUnitState>
    {
        private readonly int skill = Define.SkillAnimationHash;

        public void OnEnter(PlayerUnitController owner)
        {
            owner.Agent.isStopped = true;
            owner.Agent.velocity = Vector3.zero;
            owner.Agent.ResetPath();
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
        }
    }
}