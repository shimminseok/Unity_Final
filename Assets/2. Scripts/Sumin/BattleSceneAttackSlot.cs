using UnityEngine;
using UnityEngine.UI;

public class BattleSceneAttackSlot : MonoBehaviour
{
    [SerializeField] private GameObject highLight;

    // 기본공격 버튼 클릭하면 전달
    public void OnClickBasicAttack()
    {
        InputManager.Instance.SelectBasicAttack();
    }

    public void ToggleHighlightAttackBtn(bool toggle)
    {
        highLight.SetActive(toggle);
    }
}
