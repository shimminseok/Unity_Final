public static class EmotionFactory
{
    public static BaseEmotion CreateEmotion(EmotionType emotionType)
    {
        return emotionType switch
        {
            EmotionType.None       => new NeutralEmotion(),
            EmotionType.Anger      => new AngerEmotion(),
            EmotionType.Depression => new DepressionEmotion(),
            EmotionType.Joy        => new JoyEmotion(),

            _ => null
        };
    }
}