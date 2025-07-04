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


    public static string GetStatName(StatType statType)
    {
        return statType switch
        {
            StatType.MaxHp        => "최대 HP",
            StatType.AttackPow    => "공격력",
            StatType.Counter      => "반격 확률",
            StatType.Defense      => "방어력",
            StatType.Speed        => "속도",
            StatType.CriticalRate => "치명타 확률",
            StatType.CriticalDam  => "치명타 대미지",
            StatType.HitRate      => "명중률",

            _ => string.Empty
        };
    }
}