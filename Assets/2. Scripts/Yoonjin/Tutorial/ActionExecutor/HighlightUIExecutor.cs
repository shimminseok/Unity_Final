using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightUIExecutor : TutorialActionExecutor
{
    public override void Enter(TutorialActionData actionData)
    {
        var data = actionData as HighlightUIActionData;
        if (data == null) return;

        if (data.autoBlockOthers)
        {
            TutorialUIBlocker.BlockAllExcept(data.targetButton);
        }
        else
        {
            TutorialUIBlocker.BlockAllExcept(data.targetButton, data.exceptionButtons);
        }

        TutorialUIHighlighter.Highlight(data.targetButton);
    }

    public override void Exit()
    {
        TutorialUIBlocker.Clear();
        TutorialUIHighlighter.Clear();
    }
}
