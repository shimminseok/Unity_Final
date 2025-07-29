using UnityEngine;
using UnityEngine.UI;

public class HighlightUIExecutor : TutorialActionExecutor
{
    private Button targetButton;

    private bool requireDoubleClick;
    private int clickCount = 0;

    private bool requireLongPress;
    private HoldPressDetector holdPressDetector;

    public override void Enter(TutorialActionData actionData)
    {
        var data = actionData as HighlightUIActionData;
        if (data == null) return;

        requireDoubleClick = data.requireDoubleClick;
        clickCount = 0;

        requireLongPress = data.requireHold;

        targetButton = GameObject.Find(data.targetButtonName)?.GetComponent<Button>();

        if (targetButton == null)
        {
            Debug.LogError($"[튜토리얼] '{data.targetButtonName}' 버튼을 찾을 수 없습니다.");
            return;
        }

        if (requireLongPress)
        {
            // 버튼의 기본 클릭 동작을 비활성화
            targetButton.interactable = false;

            holdPressDetector = targetButton.gameObject.GetComponent<HoldPressDetector>();
            if (holdPressDetector == null)
                holdPressDetector = targetButton.gameObject.AddComponent<HoldPressDetector>();

            holdPressDetector.requireHoldTime = 0.5f;
            holdPressDetector.OnLongPress += OnLongPressed;
        }
        else
        {
            // 클릭 이벤트로 다음 단계 진행
            targetButton.onClick.AddListener(OnTargetButtonClicked);
        }

        if (data.autoBlockOthers)
        {
            TutorialUIBlocker.BlockAllExcept(targetButton.gameObject);
        }
        TutorialUIHighlighter.Highlight(targetButton.gameObject);
    }

    private void OnTargetButtonClicked()
    {
        clickCount++;

        if (requireDoubleClick && clickCount < 2)
            return;

        // 다음 Step 진행
        manager.CompleteCurrentStep();
    }

    private void OnLongPressed()
    {
        // 버튼 재활성화
        targetButton.interactable = true;

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


