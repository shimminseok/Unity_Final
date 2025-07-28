using UnityEngine;

public class AnimationDone : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Unit unit = animator.GetComponent<Unit>();
        if (stateInfo.IsTag("Attack"))
        {
            if (unit.CurrentAttackAction.DistanceType == AttackDistanceType.Melee)
            {
                unit.InvokeAttackFinished();
            }
        }

        else if (stateInfo.IsTag("Hit"))
        {
            unit.InvokeHitFinished();
        }

        else if (stateInfo.IsTag("Skill"))
        {
            unit.InvokeSkillFinished();
        }
    }
}