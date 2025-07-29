using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : Singleton<TutorialManager>
{
    [SerializeField] private TutorialTable tutorialTable;

    // 행동별 실행기 매핑 (FSM처럼 동작)
    private Dictionary<TutorialActionType, TutorialActionExecutor> executorMap;

    private TutorialStepSO currentStep;
    public TutorialStepSO CurrentStep => currentStep;

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
        IsActive = true;
        // 튜토리얼 첫 단계 실행
        // GoToStep(0);
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

        GoToStep(currentStep.NextID);
        Debug.Log("튜토리얼 다음 단계로");
    }

    // 튜토리얼 종료
    public void EndTutorial()
    {
        IsActive = false;

        // RewardManager.Instance.GiveReward 같은 걸로 추후 보상 지급 예정

        Debug.LogWarning("튜토리얼 종료!");
    }

    public void SetTutorialIndex(int index)
    {
        GoToStep(index);
    }
}