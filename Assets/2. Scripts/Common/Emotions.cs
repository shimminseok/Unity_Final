public abstract class BaseEmotion
{
    public Emotion EmotionType;
    public int Stack;

    public abstract void Execute();
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
        //실행되는걸 해줌
        //확률 높여준다거나
        //뭔짓을 한다.
        //감정에 맞는 이상한짓을 할꺼임.
        Stack++;
    }
}