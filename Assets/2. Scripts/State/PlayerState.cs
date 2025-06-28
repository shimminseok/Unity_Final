using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerState
{
    public class IdleState : IState<PlayerUnitController, PlayerUnitState>
    {
        private static readonly int IsMove = Animator.StringToHash("IsMove");

        public void OnEnter(PlayerUnitController owner)
        {
            owner.Animator.SetBool(IsMove, false);
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
        private static readonly int IsMoving = Animator.StringToHash("IsMove");

        public void OnEnter(PlayerUnitController owner)
        {
            owner.setRemainDistance = 1.5f;
            Debug.Log("Enter Move State");
            owner.Animator.SetBool(IsMoving, true);
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
            entity.Animator.SetBool(IsMoving, false);
        }
    }

    public class ReturnState : IState<PlayerUnitController, PlayerUnitState>
    {
        private static readonly int IsMoving = Animator.StringToHash("IsMove");

        public void OnEnter(PlayerUnitController owner)
        {
            owner.setRemainDistance = 0.1f;
            owner.transform.LookAt(owner.StartPostion, Vector3.up);
            owner.Animator.SetBool(IsMoving, true);
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
            owner.Animator.SetBool(IsMoving, false);
            owner.transform.localRotation = Quaternion.identity;
        }
    }

    public class AttackState : IState<PlayerUnitController, PlayerUnitState>
    {
        private static readonly int Attack = Animator.StringToHash("Attack");

        public void OnEnter(PlayerUnitController owner)
        {
            owner.Animator.SetTrigger(Attack);
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
            entity.Animator.ResetTrigger(Attack);
        }
    }

    public class SkillState : IState<PlayerUnitController, PlayerUnitState>
    {
        private static readonly int Skill = Animator.StringToHash("Skill");

        public void OnEnter(PlayerUnitController owner)
        {
            owner.Animator.SetTrigger(Skill);
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
            owner.Animator.ResetTrigger(Skill);
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