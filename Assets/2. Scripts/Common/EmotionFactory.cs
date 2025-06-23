public static class EmotionFactory
{
    public static BaseEmotion CreateEmotion(Emotion emotion)
    {
        return emotion switch
        {
            Emotion.Anger => new AngerEmotion(),


            _ => null
        };
    }
}