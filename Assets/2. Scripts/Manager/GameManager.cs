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
    }
}