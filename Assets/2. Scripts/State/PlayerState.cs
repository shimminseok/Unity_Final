using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerState
{
    public class IdleState : IState<PlayerUnitController, PlayerUnitState>
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

    public class MoveState : IState<PlayerUnitController, PlayerUnitState>
    {
        public void OnEnter(PlayerUnitController owner)
        {
            Debug.Log("Enter Move State");
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
        }
    }

    public class ReturnState : IState<PlayerUnitController, PlayerUnitState>
    {
        public void OnEnter(PlayerUnitController owner)
        {
            Debug.Log("Enter Move State");
        }

        public void OnUpdate(PlayerUnitController owner)
        {
            owner.MoveTo(owner.StartPostion);
        }

        public void OnFixedUpdate(PlayerUnitController owner)
        {
        }

        public void OnExit(PlayerUnitController entity)
        {
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