using UnityEngine;

public abstract class BaseEmotion
{
    public EmotionType EmotionType;
    public int Stack;

    public abstract void Enter(Unit unit);
    public abstract void Execute(Unit unit);
    public abstract void Exit(Unit unit);

    public void AddStack(int amount = 1)
    {
        Stack += amount;
    }
}

public class JoyEmotion : BaseEmotion, IEmotionOnHitChance
{
    private const float CritDamageUpMin = 0.1f;
    private const float CritDamageUpMax = 0.4f;
    private const float MissChanceMax = 0.3f;
    private const float missChanceAmount = 0.05f;
    private const float critUpAmount = 0.1f;


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
        Debug.Log("기쁨 상태 종료!!");
    }

    public void OnCalculateHitChance(ref float hitRate)
    {
        float chance = Mathf.Min(Stack * missChanceAmount, MissChanceMax);
        hitRate = Mathf.Clamp01(hitRate - chance);
    }
}

public class AngerEmotion : BaseEmotion, IEmotionOnAttack
{
    private const float AttackUpMin = 0.1f;
    private const float AttackUpMax = 0.3f;
    private const float AllyHitChanceMax = 0.15f;
    private const float attackUpAmount = 0.05f;

    private float allyHitChance;

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
        Debug.Log("분노 상태 종료!!");
    }

    public void OnBeforeAttack(Unit attacker, ref IDamageable target)
    {
        float chance = Mathf.Min(Stack * attackUpAmount, AllyHitChanceMax);

        if (Random.value < chance)
        {
            //target을 아군으로 바꿔줌
            //BattleManager에 있는 아군 리스트를 가져와서 다시 target을 지정해줄꺼
            var allies = BattleManager.Instance.GetAllies(attacker);
            target = allies[Random.Range(0, allies.Count)];
            Debug.Log("아군 공격함!");
        }
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
        Stack++;
    }

    public override void Exit(Unit unit)
    {
        Stack = 0;
        Debug.Log("노말 상태 종료!!");
    }
}

public class DepressionEmotion : BaseEmotion, IEmotionOnTakeDamage
{
    private const float DefenseDownMin = 0.1f;
    private const float DefenseDownMax = 0.3f;
    private const float InvincibleChanceMax = 0.1f;
    private const float invincibleChance = 0.02f;

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
        Stack++;
    }

    public override void Exit(Unit unit)
    {
        Stack = 0;
        Debug.Log("우울 상태 종료!!");
    }

    public void OnBeforeTakeDamage(Unit unit, ref float damage, out bool ignoreDamage)
    {
        float chance = Mathf.Min(Stack * invincibleChance, InvincibleChanceMax);
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
}