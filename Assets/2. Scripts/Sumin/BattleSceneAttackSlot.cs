using UnityEngine;

public class BattleSceneAttackSlot : MonoBehaviour
{
    public void OnClickBasicAttack()
    {
        InputManager.Instance.SelectBasicAttack();
    }
}
