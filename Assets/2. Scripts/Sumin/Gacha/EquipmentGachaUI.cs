using UnityEngine;
using UnityEngine.UI;

public class EquipmentGachaUI : UIBase
{
    [SerializeField] private EquipmentGachaSystem gachaSystem;

    [Header("뽑기 버튼")]
    [SerializeField] private Button oneDrawBtn;
    [SerializeField] private Button tenDrawBtn;

    [Header("뽑기 확인 창")]
    [SerializeField] private GachaConfirmPopupUI confirmPanel;

    [Header("뽑기 결과 창")]
    [SerializeField] private EquipmentGachaResultUI resultPanel;

    private UIManager uiManager;
    private GachaCantDrawPopupUI cantDrawPopupUI;
    private EquipmentGachaResultUI resultUI;

    private int drawCount;

    private void Start()
    {
        oneDrawBtn.onClick.RemoveAllListeners();
        oneDrawBtn.onClick.AddListener(() => OnDrawCountBtn(1));

        tenDrawBtn.onClick.RemoveAllListeners();
        tenDrawBtn.onClick.AddListener(() => OnDrawCountBtn(10));

        uiManager = UIManager.Instance;
        cantDrawPopupUI = uiManager.GetUIComponent<GachaCantDrawPopupUI>();
        resultUI = uiManager.GetUIComponent<EquipmentGachaResultUI>();
    }

    private void OnDisable()
    {
        confirmPanel.OnConfirm -= HandleConfirm;
        confirmPanel.OnConfirm -= HandleCancel;
    }

    public void OnDrawCountBtn(int count)
    {
        if (!gachaSystem.CheckCanDraw(count))
        {
            uiManager.Open(cantDrawPopupUI);
            return;
        }
        drawCount = count;

        confirmPanel.OnConfirm += HandleConfirm;
        confirmPanel.OnConfirm += HandleCancel;
        confirmPanel.ShowPopup(gachaSystem.DrawCost * count);
    }

    private void HandleConfirm()
    {
        DrawAndDisplayResult(drawCount);
    }

    private void HandleCancel()
    {
        Debug.Log("취소됨");
    }

    private void DrawAndDisplayResult(int count)
    {
        EquipmentItemSO[] equipments = gachaSystem.DrawEquipments(count);

        uiManager.Open(resultUI);
        resultPanel.ShowEquipments(equipments);
    }
}
