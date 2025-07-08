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

    // 팝업 띄우고, ok 버튼 누르면 뽑기 진행, cancel 버튼 누르면 팝업 닫기
    public void ShowPopup(int drawCost)
    {
        Open();
        drawCostText.text = $"{drawCost}"; // 뽑기 비용 표시

        okBtn.onClick.RemoveAllListeners();
        cancelBtn.onClick.RemoveAllListeners();

        okBtn.onClick.AddListener(() =>
        {
            OnConfirm?.Invoke();
            ClearEvents();
            Close();
        });

        cancelBtn.onClick.AddListener(() =>
        {
            OnCancel?.Invoke();
            ClearEvents();
            Close();
        });
    }

    // 이벤트 누적 방지
    private void ClearEvents()
    {
        OnConfirm = null;
        OnCancel = null;
    }
}
