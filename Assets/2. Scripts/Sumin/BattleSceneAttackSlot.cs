using UnityEngine;
using UnityEngine.UI;

public class BattleSceneAttackSlot : MonoBehaviour
{
    private Button attackBtn;

    private void Start()
    {
        attackBtn = GetComponent<Button>();
    }
    // 기본공격 버튼 클릭하면 전달
    public void OnClickBasicAttack()
    {
        InputManager.Instance.SelectBasicAttack();
    }

    public void HighlightAttackBtn()
    {
        ColorBlock colorBlock = attackBtn.colors;
        colorBlock.normalColor = new Color(1, 1, 1);
        Debug.Log("공격 하이라이트");
    }
}
