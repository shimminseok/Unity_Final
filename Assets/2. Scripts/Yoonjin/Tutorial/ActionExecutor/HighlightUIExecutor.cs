using UnityEngine;
using UnityEngine.UI;

public class HighlightUIExecutor : TutorialActionExecutor
{
    public override void Enter(TutorialActionData actionData)
    {
        var data = actionData as HighlightUIActionData;
        if (data == null) return;

        var target = GameObject.Find(data.targetButtonName)?.GetComponent<Button>();
        if (target == null)
        {
            Debug.LogError($"[튜토리얼] '{data.targetButtonName}' 버튼을 찾을 수 없습니다.");
            return;
        }

        TutorialUIBlocker.BlockAllExcept(target.gameObject);

        TutorialUIHighlighter.Highlight(target.gameObject);
    }

    public override void Exit()
    {
        TutorialUIBlocker.Clear();
        TutorialUIHighlighter.Clear();
    }
}

