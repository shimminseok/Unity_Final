using System.Collections.Generic;
using UnityEngine;

public static class Define
{
    public static readonly int MoveAnimationHash = Animator.StringToHash("IsMove");
    public static readonly int AttackAnimationHash = Animator.StringToHash("Attack");
    public static readonly int SkillAnimationHash = Animator.StringToHash("Skill");
    public static readonly int DeadAnimationHash = Animator.StringToHash("Dead");
    public static readonly int VictoryAnimationHash = Animator.StringToHash("Victory");
    public static readonly int ReadyActionAnimationHash = Animator.StringToHash("ReadyAction");


    public static readonly string IdleClipName = "Idle";
    public static readonly string MoveClipName = "Move";
    public static readonly string AttackClipName = "Attack";
    public static readonly string SkillClipName = "Skill";
    public static readonly string DeadClipName = "Die";
    public static readonly string VictoryClipName = "Victory";
    public static readonly string ReadyActionClipName = "ReadyAction";

    // 티어 별 가챠 확률
    public static readonly Dictionary<Tier, float> TierRates = new()
    {
        { Tier.A, 90f },
        { Tier.S, 9f },
        { Tier.SR, 0.98f },
        { Tier.SSR, 0.02f }
    };
}