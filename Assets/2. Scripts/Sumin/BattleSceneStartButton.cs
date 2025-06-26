using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSceneStartButton : MonoBehaviour
{
    public void OnStartButton()
    {
        BattleManager.Instance.StartTurn();
    }
}
