using UnityEngine;

public static class Define
{
    public static readonly int MoveAnimationHash = Animator.StringToHash("IsMove");
    public static readonly int AttackAnimationHash = Animator.StringToHash("Attack");
    public static readonly int SkillAnimationHash = Animator.StringToHash("Skill");

    public static readonly string PlayerAttackClipName = "ATK0";
}