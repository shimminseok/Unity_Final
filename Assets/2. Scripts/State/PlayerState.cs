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

        public PlayerUnitState CheckTransition(PlayerUnitController owner)
        {
            if (owner.IsStunned)
                return PlayerUnitState.Stun;
            else if (owner.IsDead)
                return PlayerUnitState.Die;
            return PlayerUnitState.Idle;
        }
    }

    public class AttackState : IState<PlayerUnitController, PlayerUnitState>
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

        public PlayerUnitState CheckTransition(PlayerUnitController owner)
        {
            if (owner.IsStunned)
                return PlayerUnitState.Stun;
            else if (owner.IsDead)
                return PlayerUnitState.Die;

            return PlayerUnitState.Attack;
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

        public PlayerUnitState CheckTransition(PlayerUnitController owner)
        {
            if (owner.IsStunned)
                return PlayerUnitState.Stun;
            else if (owner.IsDead)
                return PlayerUnitState.Die;

            return PlayerUnitState.Skill;
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

        public PlayerUnitState CheckTransition(PlayerUnitController owner)
        {
            return PlayerUnitState.Die;
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

        public PlayerUnitState CheckTransition(PlayerUnitController owner)
        {
            if (owner.IsDead)
                return PlayerUnitState.Die;

            return PlayerUnitState.Idle;
        }
    }

    public class EndTurnState : IState<PlayerUnitController, PlayerUnitState>
    {
        public void OnEnter(PlayerUnitController owner)
        {
            BattleManager.Instance.TurnHandler.OnUnitTurnEnd();
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

        public PlayerUnitState CheckTransition(PlayerUnitController owner)
        {
            return PlayerUnitState.Idle;
        }
    }
}