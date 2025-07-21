using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TriggerWaitExecutor : TutorialActionExecutor
{
    private string eventName;

    public override void Enter(TutorialActionData actionData)
    {
        var data = actionData as TriggerWaitActionData;
        if (data == null) return;

        eventName = data.triggerEventName;
        EventBus.Subscribe(eventName, OnTrigger);
    }

    private void OnTrigger()
    {
        EventBus.Unsubscribe(eventName, OnTrigger);
        manager.CompleteCurrentStep();
    }

    public override void Exit()
    {
        EventBus.Unsubscribe(eventName, OnTrigger);
    }
}
