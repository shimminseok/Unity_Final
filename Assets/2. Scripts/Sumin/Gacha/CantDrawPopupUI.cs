using UnityEngine;
using UnityEngine.UI;

public class CantDrawPopupUI : UIBase
{
    [SerializeField] private Button exitBtn;

    private void Start()
    {
        exitBtn.onClick.RemoveAllListeners();
        exitBtn.onClick.AddListener(OnClickExitBtn);
    }

    public void OnClickExitBtn()
    {
        Close();
    }
}
