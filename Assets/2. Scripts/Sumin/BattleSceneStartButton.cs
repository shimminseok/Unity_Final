using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSceneStartButton : UIBase
{
    public void OnStartButton()
    {
        InputManager.Instance.OnClickTurnStartButton();
    }
}
