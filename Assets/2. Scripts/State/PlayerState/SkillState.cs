using UnityEngine;

namespace PlayerState
{
    public class SkillState : IState<PlayerUnitController, PlayerUnitState>
    {
        private readonly int skill = Define.SkillAnimationHash;

        public void OnEnter(PlayerUnitController owner)
        {
            owner.OnToggleNavmeshAgent(false);
            owner.IsAnimationDone = false;
            TimeLineManager.Instance.PlayTimeLine(CameraManager.Instance.cinemachineBrain, CameraManager.Instance.skillCameraController, owner, out bool isTimeLine);

            if (!isTimeLine)
            {
                owner.Animator.SetTrigger(Define.SkillAnimationHash);
                owner.SkillController.CurrentSkillData.skillSo.skillType.PlayCastVFX(owner, owner.Target);
                owner.SkillController.CurrentSkillData.skillSo.skillType.PlayCastSFX(owner);
            }
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