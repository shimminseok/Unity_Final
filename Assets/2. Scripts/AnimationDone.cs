using UnityEngine;

public class AnimationDone : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var unit = animator.GetComponent<Unit>();

        if (unit != null)
        {
            unit.IsAnimationDone = true;
        }
    }
}