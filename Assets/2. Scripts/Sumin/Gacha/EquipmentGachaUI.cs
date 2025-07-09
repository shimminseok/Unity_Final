using UnityEngine;
using UnityEngine.UI;

public class EquipmentGachaUI : MonoBehaviour
{
    [SerializeField] private EquipmentGachaSystem gachaSystem;

    [Header("뽑기 버튼")]
    [SerializeField] private Button oneDrawBtn;
    [SerializeField] private Button tenDrawBtn;

    [Header("뽑기 확인 창")]
    [SerializeField] private GachaConfirmPopupUI confirmPanel;

    [Header("뽑기 결과 창")]
    [SerializeField] private SkillGachaResultUI resultPanel;

    private GachaCantDrawPopupUI cantDrawPopupUI;
    private UIManager uiManager;

    private void Start()
    {
        oneDrawBtn.onClick.RemoveAllListeners();
        oneDrawBtn.onClick.AddListener(() => OnDrawCountBtn(1));

        tenDrawBtn.onClick.RemoveAllListeners();

        uiManager = UIManager.Instance;
        cantDrawPopupUI = uiManager.GetUIComponent<GachaCantDrawPopupUI>();
        
    }

    public void OnDrawCountBtn(int count)
    {
        if (!gachaSystem.CheckCanDraw(count))
        {
            uiManager.Open(cantDrawPopupUI);
            return;
        }
        confirmPanel.OnConfirm += () => DrawAndDisplayResult(count);
    }

    private void DrawAndDisplayResult(int count)
    {
        EquipmentItemSO[] equipments = gachaSystem.DrawEquipments(count);

        resultPanel.Open();
        //resultPanel.ShowEquipments(equipments);
    }
}
