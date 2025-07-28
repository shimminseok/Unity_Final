namespace EnemyState
{
    public class HitState : IState<EnemyUnitController, EnemyUnitState>
    {
        private readonly int isHit = Define.HitAnimationHash;

        public void OnEnter(EnemyUnitController owner)
        {
            owner.Animator.SetTrigger(isHit);
        }

        public void OnUpdate(EnemyUnitController owner)
        {
        }

        public void OnFixedUpdate(EnemyUnitController owner)
        {
        }

        public void OnExit(EnemyUnitController owner)
        {
        }
    }
}