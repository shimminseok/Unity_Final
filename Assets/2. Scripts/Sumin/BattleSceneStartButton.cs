using UnityEngine;
using UnityEngine.UI;

public class BattleSceneStartButton : UIBase
{
    [SerializeField] private Button startBtn;
    public void DisableStartButton()
    {
        startBtn.interactable = false;
    }

    public void EnableStartButton()
    {
        startBtn.interactable = true;
    }

    public void OnStartButton()
    {
        InputManager.Instance.OnClickTurnStartButton();
    }
}
