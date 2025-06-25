using UnityEngine;

public abstract class BaseEmotion
{
    public EmotionType EmotionType;
    public int Stack;
    protected const int MaxStack = 10;

    public abstract void Enter(Unit unit);
    public abstract void Execute(Unit unit);
    public abstract void Exit(Unit unit);

    public void AddStack(Unit unit, int amount = 1)
    {
        Stack += amount;
        Stack = Mathf.Clamp(Stack, 0, MaxStack);
        OnStackChanged(unit);
    }

    public virtual void OnStackChanged(Unit unit)
    {
    }
}

public class JoyEmotion : BaseEmotion, IEmotionOnHitChance
{
    private const float CritDamageUpMax = 0.4f;
    private const float CritDamageUpPerStack = CritDamageUpMax / MaxStack;

    private const float MissChanceMax = 0.3f;
    private const float MissChanceAmount = 0.05f;


    private float critDamUpAmount = 0f;

    public JoyEmotion()
    {
        EmotionType = EmotionType.Joy;
        Stack = 0;
    }

    public override void Enter(Unit unit)
    {
        Debug.Log("기쁨 상태 진입!!");
    }

    public override void Execute(Unit unit)
    {
    }

    public override void Exit(Unit unit)
    {
        Stack = 0;
        unit.StatManager.ApplyStatEffect(StatType.AttackPow, StatModifierType.BuffPercent, -critDamUpAmount);
        critDamUpAmount = 0;
        Debug.Log("기쁨 상태 종료!!");
    }

    public void OnCalculateHitChance(ref float hitRate)
    {
        float chance = Mathf.Min(Stack * MissChanceAmount, MissChanceMax);
        hitRate = Mathf.Clamp01(hitRate - chance);
    }

    public override void OnStackChanged(Unit unit)
    {
        // 1. 기존 버프 제거
        unit.StatManager.ApplyStatEffect(StatType.CriticalDam, StatModifierType.BuffPercent, -critDamUpAmount);
        // 2. 새 버프 계산
        critDamUpAmount = Mathf.Min(Stack * CritDamageUpPerStack, CritDamageUpMax);
        // 3. 새 버프 적용
        unit.StatManager.ApplyStatEffect(StatType.CriticalDam, StatModifierType.BuffPercent, critDamUpAmount);
    }
}

public class AngerEmotion : BaseEmotion, IEmotionOnAttack
{
    private const float AttackUpMax = 0.3f;
    private const float AttackUpPerStack = AttackUpMax / MaxStack;
    private const float AllyHitChanceMax = 0.15f;
    private const float AllyAttackChancePerStack = 0.05f;

    private float attackUpAmount;

    public AngerEmotion()
    {
        EmotionType = EmotionType.Anger;
        Stack = 0;
    }

    public override void Enter(Unit unit)
    {
        Debug.Log("분노 상태 진입!!");
    }

    public override void Execute(Unit unit)
    {
    }

    public override void Exit(Unit unit)
    {
        Stack = 0;
        unit.StatManager.ApplyStatEffect(StatType.AttackPow, StatModifierType.BuffPercent, -attackUpAmount);
        attackUpAmount = 0;
        Debug.Log("분노 상태 종료!!");
    }

    public void OnBeforeAttack(Unit attacker, ref IDamageable target)
    {
        float chance = Mathf.Min(Stack * AllyAttackChancePerStack, AllyHitChanceMax);

        if (Random.value < chance)
        {
            //타겟을 아군으로 바꿔줌
            var allies = BattleManager.Instance.GetAllies(attacker);
            target = allies[Random.Range(0, allies.Count)];
            Debug.Log("아군 공격함!");
        }
    }

    public override void OnStackChanged(Unit unit)
    {
        // 1. 기존 버프 제거
        unit.StatManager.ApplyStatEffect(StatType.AttackPow, StatModifierType.BuffPercent, -attackUpAmount);
        // 2. 새 버프 계산
        attackUpAmount = Mathf.Min(Stack * AttackUpPerStack, AttackUpMax);
        // 3. 새 버프 적용
        unit.StatManager.ApplyStatEffect(StatType.AttackPow, StatModifierType.BuffPercent, attackUpAmount);
    }
}

public class NeutralEmotion : BaseEmotion
{
    public NeutralEmotion()
    {
        EmotionType = EmotionType.None;
        Stack = 0;
    }

    public override void Enter(Unit unit)
    {
        Debug.Log("노말 상태 진입!!");
    }

    public override void Execute(Unit unit)
    {
    }

    public override void Exit(Unit unit)
    {
        Stack = 0;
        Debug.Log("노말 상태 종료!!");
    }
}

public class DepressionEmotion : BaseEmotion, IEmotionOnTakeDamage
{
    private const float DefenseDownMax = 0.3f;
    private const float DefenseDownPerStack = DefenseDownMax / MaxStack;
    private const float InvincibleChanceMax = 0.1f;
    private const float InvincibleChance = 0.02f;

    private float defenseDownAmount;


    public DepressionEmotion()
    {
        EmotionType = EmotionType.Depression;
        Stack = 0;
    }

    public override void Enter(Unit unit)
    {
        Debug.Log("우울 상태 진입!!");
    }

    public override void Execute(Unit unit)
    {
    }

    public override void Exit(Unit unit)
    {
        Stack = 0;

        unit.StatManager.ApplyStatEffect(StatType.Defense, StatModifierType.BuffPercent, -defenseDownAmount);
        defenseDownAmount = 0;
        Debug.Log("우울 상태 종료!!");
    }

    public void OnBeforeTakeDamage(Unit unit, out bool ignoreDamage)
    {
        float chance = Mathf.Min(Stack * InvincibleChance, InvincibleChanceMax);
        if (Random.value < chance)
        {
            ignoreDamage = true;
            Debug.Log($"{unit.name}가 우울 상태로 무적 발동!");
        }
        else
        {
            ignoreDamage = false;
        }
    }

    public override void OnStackChanged(Unit unit)
    {
        // 1. 기존 버프 제거
        unit.StatManager.ApplyStatEffect(StatType.Defense, StatModifierType.BuffPercent, defenseDownAmount);
        // 2. 새 버프 계산
        defenseDownAmount = Mathf.Min(Stack * DefenseDownPerStack, DefenseDownMax);
        // 3. 새 버프 적용
        unit.StatManager.ApplyStatEffect(StatType.Defense, StatModifierType.BuffPercent, -defenseDownAmount);
    }
}