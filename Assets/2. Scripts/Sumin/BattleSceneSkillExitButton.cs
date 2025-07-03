using UnityEngine;

public class BattleSceneSkillExitButton : MonoBehaviour
{
    public void OnClickSkillExit()
    {
        InputManager.Instance.OnClickSkillExitButton();
    }
}
