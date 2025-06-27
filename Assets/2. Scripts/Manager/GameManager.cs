using UnityEngine;


public class GameManager : Singleton<GameManager>
{
    //Test
    [SerializeField] private PlayerSkillController skillController;

    public Unit CurrentUnit;

    private void OnGUI()
    {
        float buttonWidth  = 150f;
        float buttonHeight = 80f;
        float spacing      = 5f;

        float x = Screen.width - (buttonWidth + 10f);
        float y = Screen.height - buttonHeight - 50f;


        if (GUI.Button(new Rect(x, y - ((buttonHeight + spacing) * 1), buttonWidth, buttonHeight), "EndTurn"))
        {
            BattleManager.Instance.EndTurn();
        }

        if (GUI.Button(new Rect(x, y - ((buttonHeight + spacing) * 2), buttonWidth, buttonHeight), "StartTurn"))
        {
            BattleManager.Instance.StartTurn();
        }

        if (GUI.Button(new Rect(x, y - ((buttonHeight + spacing) * 3), buttonWidth, buttonHeight), "Skill_1"))
        {
            skillController.ChangeSkill(0);
            skillController.UseSkill();
        }

        if (GUI.Button(new Rect(x, y - ((buttonHeight + spacing) * 4), buttonWidth, buttonHeight), "Skill_2"))
        {
            skillController.ChangeSkill(1);
            skillController.UseSkill();
        }

        if (GUI.Button(new Rect(x, y - ((buttonHeight + spacing) * 5), buttonWidth, buttonHeight), "Skill_3"))
        {
            skillController.ChangeSkill(2);
            skillController.UseSkill();
        }
    }
}