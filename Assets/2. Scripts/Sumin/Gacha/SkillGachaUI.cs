using System;
using UnityEngine;
using UnityEngine.UI;

public class SkillGachaUI : UIBase
{
    [SerializeField] private SkillGachaSystem gachaSystem;

    [Header("뽑기 버튼")]
    [SerializeField] private Button oneDrawBtn;
    [SerializeField] private Button tenDrawBtn;

    [Header("뽑기 결과 창")]
    [SerializeField] private SkillGachaResultUI resultPanel;

    private UIManager uiManager;
    private SkillGachaResultUI resultUI;

    private int drawCount;

    private void Start()
    {
        oneDrawBtn.onClick.RemoveAllListeners();
        oneDrawBtn.onClick.AddListener(() => OnDrawCountBtn(1));

        tenDrawBtn.onClick.RemoveAllListeners();
        tenDrawBtn.onClick.AddListener(() => OnDrawCountBtn(10));

        uiManager = UIManager.Instance;
        resultUI = uiManager.GetUIComponent<SkillGachaResultUI>();
    }

    // n회 뽑기 버튼
    public void OnDrawCountBtn(int count)
    {
        if (!gachaSystem.CheckCanDraw(count))
        {
            PopupManager.Instance.GetUIComponent<ToastMessageUI>().SetToastMessage("Opal이 부족합니다!");
            return;
        }

        drawCount = count;

        string message = $"{count}회 스킬 소환을 진행하시겠습니까?\n 소모 Opal : {gachaSystem.DrawCost * count}";
        Action leftAction = () => DrawAndDisplayResult(drawCount);
        PopupManager.Instance.GetUIComponent<TwoChoicePopup>()?.SetAndOpenPopupUI("스킬 소환", message, leftAction, null, "소환", "취소");
    }

    // 스킬 뽑고 결과 보여주기
    private void DrawAndDisplayResult(int count)
    {
        GachaResult<ActiveSkillSO>[] skills = gachaSystem.DrawSkills(count);

        uiManager.Open(resultPanel);
        resultPanel.ShowSkills(skills);
    }
}