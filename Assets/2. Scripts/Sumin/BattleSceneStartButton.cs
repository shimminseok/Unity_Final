using UnityEngine;
using UnityEngine.UI;

public class BattleSceneStartButton : UIBase
{
    [SerializeField] private Button startBtn;
    public void DisableStartButton()
    {
        startBtn.enabled = false;
    }

    public void EnableStartButton()
    {
        startBtn.enabled = true;
    }

    public void OnStartButton()
    {
        InputManager.Instance.OnClickTurnStartButton();
    }
}
