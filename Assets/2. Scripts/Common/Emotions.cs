public abstract class BaseEmotion
{
    public Emotion EmotionType;
    public int Stack;

    public abstract void Execute();
}

public class JoyEmotion : BaseEmotion
{
    public JoyEmotion()
    {
        EmotionType = Emotion.Joy;
        Stack = 0;
    }

    public override void Execute()
    {
    }
}

public class AngerEmotion : BaseEmotion
{
    public AngerEmotion()
    {
        EmotionType = Emotion.Anger;
        Stack = 0;
    }

    public override void Execute()
    {
    }
}

public class NeutralEmotion : BaseEmotion
{
    public NeutralEmotion()
    {
        EmotionType = Emotion.Neutral;
        Stack = 0;
    }

    public override void Execute()
    {
    }
}

public class DepressionEmotion : BaseEmotion
{
    public DepressionEmotion()
    {
        EmotionType = Emotion.Depression;
        Stack = 0;
    }

    public override void Execute()
    {
    }
}