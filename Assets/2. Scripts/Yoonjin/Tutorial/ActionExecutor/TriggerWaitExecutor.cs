using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TriggerWaitExecutor : TutorialActionExecutor
{
    public override void Enter(TutorialActionData actionData)
    {
        var waitData = actionData as TriggerWaitActionData;
        if (waitData == null) return;

        // UI 인터랙션 차단
        if (waitData.blockAllUI)
            TutorialUIBlocker.BlockAll();

        switch (waitData.triggerType)
        {
            case TriggerType.SceneLoaded:
                LoadingScreenController.Instance.OnLoadingComplete += OnTriggered;
                break;
            case TriggerType.MonsterKilled:
                EventBus.Subscribe("MonsterKilled", OnTriggered);
                break;
            case TriggerType.BattleVictory:
                EventBus.Subscribe("BattleVictory", OnTriggered);
                break;
            case TriggerType.CustomEvent:
                EventBus.Subscribe(waitData.triggerEventName, OnTriggered);
                break;
        }
    }

    private void OnTriggered()
    {
        Cleanup();
        manager.CompleteCurrentStep();
    }

    private void Cleanup()
    {
        LoadingScreenController.Instance.OnLoadingComplete -= OnTriggered;
        EventBus.Unsubscribe("MonsterKilled", OnTriggered);
        EventBus.Unsubscribe("BattleVictory", OnTriggered);
    }

    public override void Exit()
    {
        Cleanup();
    }
}
