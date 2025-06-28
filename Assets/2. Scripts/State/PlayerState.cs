using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerState
{
    public class IdleState : IState<PlayerUnitController, PlayerUnitState>
    {
        private readonly int isMove = Define.MoveAnimationHash;

        public void OnEnter(PlayerUnitController owner)
        {
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

    public class MoveState : IState<PlayerUnitController, PlayerUnitState>
    {
        private readonly int isMove = Define.MoveAnimationHash;

        public void OnEnter(PlayerUnitController owner)
        {
            owner.setRemainDistance = 1.5f;
            Debug.Log("Enter Move State");
            owner.Animator.SetBool(isMove, true);
            owner.MoveTo(owner.Target.Collider.transform.position);
        }

        public void OnUpdate(PlayerUnitController owner)
        {
            owner.MoveTo(owner.Target.Collider.transform.position);
        }

        public void OnFixedUpdate(PlayerUnitController owner)
        {
        }

        public void OnExit(PlayerUnitController entity)
        {
            entity.Animator.SetBool(isMove, false);
        }
    }

    public class ReturnState : IState<PlayerUnitController, PlayerUnitState>
    {
        private readonly int isMove = Define.MoveAnimationHash;

        public void OnEnter(PlayerUnitController owner)
        {
            owner.setRemainDistance = 0.1f;
            owner.transform.LookAt(owner.StartPostion, Vector3.up);
            owner.Animator.SetBool(isMove, true);
        }

        public void OnUpdate(PlayerUnitController owner)
        {
            owner.MoveTo(owner.StartPostion);
        }

        public void OnFixedUpdate(PlayerUnitController owner)
        {
        }

        public void OnExit(PlayerUnitController owner)
        {
            owner.Animator.SetBool(isMove, false);
            owner.transform.localRotation = Quaternion.identity;
        }
    }

    public class AttackState : IState<PlayerUnitController, PlayerUnitState>
    {
        private readonly int attack = Define.AttackAnimationHash;

        public void OnEnter(PlayerUnitController owner)
        {
            owner.Animator.SetTrigger(attack);
            owner.Attack();
        }

        public void OnUpdate(PlayerUnitController owner)
        {
        }

        public void OnFixedUpdate(PlayerUnitController owner)
        {
        }

        public void OnExit(PlayerUnitController entity)
        {
            entity.Animator.ResetTrigger(attack);
        }
    }

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

    public class DeadState : IState<PlayerUnitController, PlayerUnitState>
    {
        public void OnEnter(PlayerUnitController owner)
        {
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

    public class StunState : IState<PlayerUnitController, PlayerUnitState>
    {
        public void OnEnter(PlayerUnitController owner)
        {
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