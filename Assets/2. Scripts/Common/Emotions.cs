using UnityEngine;

public abstract class BaseEmotion
{
    public EmotionType EmotionType;
    public int Stack;

    public abstract void Enter(Unit unit);
    public abstract void Execute(Unit unit);
    public abstract void Exit(Unit unit);
}

public class JoyEmotion : BaseEmotion
{
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
        Stack++;
    }

    public override void Exit(Unit unit)
    {
        Stack = 0;
        Debug.Log("기쁨 상태 종료!!");
    }
}

public class AngerEmotion : BaseEmotion
{
    private const float AttackUpMin = 0.1f;
    private const float AttackUpMax = 0.3f;
    private const float AllyHitChanceMax = 0.3f;

    private float attackUpAmount;
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
        Stack++;
    }

    public override void Exit(Unit unit)
    {
        Stack = 0;
        Debug.Log("분노 상태 종료!!");
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

public class DepressionEmotion : BaseEmotion
{
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
}