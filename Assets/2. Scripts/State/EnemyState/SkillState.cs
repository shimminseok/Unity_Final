using UnityEngine;

namespace EnemyState
{
    public class SkillState : IState<EnemyUnitController, EnemyUnitState>
    {
        private readonly int skill = Define.SkillAnimationHash;

        public void OnEnter(EnemyUnitController owner)
        {
            owner.OnToggleNavmeshAgent(false);
            owner.IsAnimationDone = false;
            TimeLineManager.Instance.PlayTimeLine(CameraManager.Instance.cinemachineBrain, CameraManager.Instance.skillCameraController, owner, out bool isTimeLine);
            if (!isTimeLine)
            {
                owner.Animator.SetTrigger(Define.SkillAnimationHash);
                owner.SkillController.CurrentSkillData.skillSo.skillType.PlayCastVFX(owner,owner.Target);
                owner.SkillController.CurrentSkillData.skillSo.skillType.PlayCastSFX(owner);
            }
        }

        public void OnUpdate(EnemyUnitController owner)
        {
        }

        public void OnFixedUpdate(EnemyUnitController owner)
        {
        }

        public void OnExit(EnemyUnitController owner)
        {
            owner.Animator.ResetTrigger(skill);
        }
    }
}