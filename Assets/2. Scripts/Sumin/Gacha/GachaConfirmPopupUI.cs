using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GachaConfirmPopupUI : UIBase
{
    [SerializeField] private TextMeshProUGUI drawCostText;

    [SerializeField] private Button okBtn;
    [SerializeField] private Button cancelBtn;

    public event Action OnConfirm;
    public event Action OnCancel;

    private UIManager uiManager;

    private void Start()
    {
        uiManager = UIManager.Instance;
    }

    // 팝업 띄우고, ok 버튼 누르면 뽑기 진행, cancel 버튼 누르면 팝업 닫기
    public void ShowPopup(int drawCost)
    {
        uiManager.Open(this);
        drawCostText.text = $"{drawCost}"; // 뽑기 비용 표시

        okBtn.onClick.RemoveAllListeners();
        cancelBtn.onClick.RemoveAllListeners();

        okBtn.onClick.AddListener(() =>
        {
            OnConfirm?.Invoke();
            uiManager.Close(this);
        });

        cancelBtn.onClick.AddListener(() =>
        {
            OnCancel?.Invoke();
            uiManager.Close(this);
        });
    }

    public override void Close()
    {
        base.Close();
        okBtn.onClick.RemoveAllListeners();
        cancelBtn.onClick.RemoveAllListeners();

        OnConfirm = null;
        OnCancel = null;
    }
}
