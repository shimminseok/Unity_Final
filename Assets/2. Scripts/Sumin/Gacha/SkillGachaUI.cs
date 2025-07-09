using UnityEngine;
using UnityEngine.UI;

public class SkillGachaUI : MonoBehaviour
{
    [SerializeField] private SkillGachaSystem gachaSystem;

    [Header("뽑기 버튼")]
    [SerializeField] private Button oneDrawBtn;

    [SerializeField] private Button tenDrawBtn;

    [Header("뽑기 확인 창")]
    [SerializeField] private GachaConfirmPopupUI confirmPanel;

    [Header("뽑기 결과 창")]
    [SerializeField] private SkillGachaResultUI resultPanel;

    private UIManager uiManager;
    private GachaCantDrawPopupUI cantDrawPopupUI;

    private void Start()
    {
        oneDrawBtn.onClick.RemoveAllListeners();
        oneDrawBtn.onClick.AddListener(() => OnDrawCountBtn(1));

        tenDrawBtn.onClick.RemoveAllListeners();
        tenDrawBtn.onClick.AddListener(() => OnDrawCountBtn(10));

        uiManager = UIManager.Instance;
        cantDrawPopupUI = uiManager.GetUIComponent<GachaCantDrawPopupUI>();
    }

    // n회 뽑기 버튼
    public void OnDrawCountBtn(int count)
    {
        if (!gachaSystem.CheckCanDraw(count))
        {
            uiManager.Open(cantDrawPopupUI);
            return;
        }

        confirmPanel.OnConfirm += () => DrawAndDisplayResult(count);
        confirmPanel.OnCancel += () => Debug.Log("취소됨");
        confirmPanel.ShowPopup(gachaSystem.DrawCost * count);
    }

    // 스킬 뽑고 결과 보여주기
    private void DrawAndDisplayResult(int count)
    {
        GachaResult<ActiveSkillSO>[] skills = gachaSystem.DrawSkills(count);

        resultPanel.Open();
        resultPanel.ShowSkills(skills);
    }
}