using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : Singleton<TutorialManager>
{
    public enum TutorialPhase
    {
        DeckBuildingBefore = 0,
        DeckBuildingAfter = 1,
        LevelUp = 2
    }

    [SerializeField] private TutorialTable tutorialTable;

    // 행동별 실행기 매핑 (FSM처럼 동작)
    private Dictionary<TutorialActionType, TutorialActionExecutor> executorMap;

    private TutorialStepSO currentStep;
    public TutorialStepSO CurrentStep => currentStep;

    [HideInInspector]
    public bool IsActive;

    protected override void Awake()
    {
        base.Awake();

        // 각 행동에 대한 실행기 등록
        executorMap = new Dictionary<TutorialActionType, TutorialActionExecutor>
        {
            { TutorialActionType.Dialogue, new DialogueActionExecutor() },
            { TutorialActionType.HighlightUI, new HighlightUIExecutor() },
            { TutorialActionType.TriggerWait, new TriggerWaitExecutor() },
            { TutorialActionType.Reward, new RewardActionExecutor() }
        };

        // 실행기에 튜토리얼 매니저 주입
        foreach (TutorialActionExecutor exec in executorMap.Values)
        {
            exec.SetManager(this);
        }

        // 테이블 가져오기 
        tutorialTable = TableManager.Instance.GetTable<TutorialTable>();
    }


    private void Start()
    {
        IsActive = false;

        var tutorialData = SaveLoadManager.Instance
            .SaveDataMap.GetValueOrDefault(SaveModule.Tutorial) as SaveTutorialData;

        // 이미 튜토리얼 완료한 유저는 실행 안 함
        if (tutorialData?.IsCompleted == true)
        {
            IsActive = false;
            Debug.Log("[튜토리얼] 이미 완료된 유저입니다.");
            return;
        }

        IsActive = true;

        int resumeStep = tutorialData?.Phase switch
        {
            TutorialPhase.LevelUp => 81,
            TutorialPhase.DeckBuildingAfter => 72,
            _ => 0
        };

        GoToStep(resumeStep);
    }

    // 특정 ID의 튜토리얼 스텝 실행
    public void GoToStep(int id)
    {
        Debug.Log($"[튜토리얼] GoToStep 호출됨 (ID: {id})");

        if (!tutorialTable.DataDic.TryGetValue(id, out currentStep))
        {
            EndTutorial();
            return;
        }

        if (currentStep.ActionData == null)
        {
            Debug.LogError($"[튜토리얼] Step {id}의 ActionData가 null입니다!");
            return;
        }

        Debug.Log($"[튜토리얼] Step {id}의 ActionType: {currentStep.ActionData.ActionType}");

        if (!executorMap.TryGetValue(currentStep.ActionData.ActionType, out TutorialActionExecutor executor))
        {
            Debug.LogError($"[튜토리얼] 해당 ActionType({currentStep.ActionData.ActionType})에 대한 실행기가 없습니다!");
            return;
        }

        Debug.Log($"[튜토리얼] {currentStep.ActionData.ActionType} 실행기 호출!");
        executor.Enter(currentStep.ActionData);
    }


    // 현재 스텝을 종료하고 다음 스텝으로 전환
    public void CompleteCurrentStep()
    {
        TutorialActionExecutor executor = executorMap[currentStep.ActionData.ActionType];
        executor?.Exit();

        // 진행 중간 저장
        SaveLoadManager.Instance.SaveModuleData(SaveModule.Tutorial);

        GoToStep(currentStep.NextID);
        Debug.Log("튜토리얼 다음 단계로");
    }

    // 튜토리얼 종료
    public void EndTutorial()
    {
        IsActive = false;

        // 튜토리얼 완료 저장
        if (SaveLoadManager.Instance.SaveDataMap.GetValueOrDefault(SaveModule.Tutorial) is SaveTutorialData tutorialData)
        {
            tutorialData.IsCompleted = true;
            SaveLoadManager.Instance.SaveModuleData(SaveModule.Tutorial);
        }

        // 튜토리얼 끝났을 때만 아이템/스킬 저장
        SaveLoadManager.Instance.SaveModuleData(SaveModule.InventoryItem);
        SaveLoadManager.Instance.SaveModuleData(SaveModule.InventorySkill);

        Debug.LogWarning("튜토리얼 종료!");
    }
}
