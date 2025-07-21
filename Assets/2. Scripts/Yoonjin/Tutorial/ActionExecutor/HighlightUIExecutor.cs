using UnityEngine;
using UnityEngine.UI;

public class HighlightUIExecutor : TutorialActionExecutor
{
    private Button targetButton;

    public override void Enter(TutorialActionData actionData)
    {
        var data = actionData as HighlightUIActionData;
        if (data == null) return;

        targetButton = GameObject.Find(data.targetButtonName)?.GetComponent<Button>();
        if (targetButton == null)
        {
            Debug.LogError($"[튜토리얼] '{data.targetButtonName}' 버튼을 찾을 수 없습니다.");
            return;
        }

        TutorialUIBlocker.BlockAllExcept(targetButton.gameObject);
        TutorialUIHighlighter.Highlight(targetButton.gameObject);

        // 클릭 이벤트에 반응해서 다음 단계로 진행
        targetButton.onClick.AddListener(OnTargetButtonClicked);
    }

    private void OnTargetButtonClicked()
    {
        // 다음 Step 진행
        manager.CompleteCurrentStep();
    }

    public override void Exit()
    {
        // 클릭 리스너 해제
        if (targetButton != null)
            targetButton.onClick.RemoveListener(OnTargetButtonClicked);

        TutorialUIBlocker.Clear();
        TutorialUIHighlighter.Clear();
    }
}


