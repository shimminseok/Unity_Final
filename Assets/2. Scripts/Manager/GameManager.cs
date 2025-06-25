using UnityEngine;


public class GameManager : Singleton<GameManager>
{
    //Test
    public EmotionType testEmotion;
    [SerializeField] private Unit unit;

    private void OnGUI()
    {
        float buttonWidth  = 150f;
        float buttonHeight = 80f;
        float spacing      = 5f;

        float x = Screen.width - (buttonWidth + 10f);
        float y = Screen.height - buttonHeight - 50f;

        if (GUI.Button(new Rect(x, y - ((buttonHeight + spacing) * 0), buttonWidth, buttonHeight), "Attack"))
        {
            unit.Attack();
        }

        if (GUI.Button(new Rect(x, y - ((buttonHeight + spacing) * 1), buttonWidth, buttonHeight), "EndTurn"))
        {
            unit.EndTurn();
        }

        if (GUI.Button(new Rect(x, y - ((buttonHeight + spacing) * 2), buttonWidth, buttonHeight), "Start"))
        {
            BattleManager.Instance.StartTurn();
        }

        if (GUI.Button(new Rect(10, y - ((buttonHeight + spacing) * 0), buttonWidth, buttonHeight), "노말"))
        {
            unit.ChangeEmotion(EmotionType.None);
        }

        if (GUI.Button(new Rect(10, y - ((buttonHeight + spacing) * 1), buttonWidth, buttonHeight), "분노"))
        {
            unit.ChangeEmotion(EmotionType.Anger);
        }

        if (GUI.Button(new Rect(10, y - ((buttonHeight + spacing) * 2), buttonWidth, buttonHeight), "기쁨"))
        {
            unit.ChangeEmotion(EmotionType.Joy);
        }


        if (GUI.Button(new Rect(10, y - ((buttonHeight + spacing) * 3), buttonWidth, buttonHeight), "우울"))
        {
            unit.ChangeEmotion(EmotionType.Depression);
        }
    }
}