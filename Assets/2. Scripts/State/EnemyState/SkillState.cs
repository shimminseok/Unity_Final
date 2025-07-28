using UnityEngine;

namespace EnemyState
{
    public class SkillState : IState<EnemyUnitController, EnemyUnitState>
    {
        private readonly int skill = Define.SkillAnimationHash;

        public void OnEnter(EnemyUnitController owner)
        {
            owner.IsAnimationDone = false;
            owner.OnToggleNavmeshAgent(false);
            owner.Animator.SetTrigger(Define.SkillAnimationHash);
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