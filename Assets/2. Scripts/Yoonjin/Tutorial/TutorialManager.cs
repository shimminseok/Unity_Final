using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private TutorialTable tutorialTable;

    // 행동별 실행기 매핑 (FSM처럼 동작)
    private Dictionary<TutorialActionType, TutorialActionExecutor> executorMap;

    private TutorialStepSO currentStep;
    public TutorialStepSO CurrentStep => currentStep;

    private void Awake()
    {
        // 각 행동에 대한 실행기 등록
        executorMap = new Dictionary<TutorialActionType, TutorialActionExecutor>
        {
            { TutorialActionType.Dialogue, new DialogueActionExecutor() },
            { TutorialActionType.HighlightUI, new HighlightUIExecutor() },
            { TutorialActionType.TriggerWait, new TriggerWaitExecutor() },
        };

        // 실행기에 튜토리얼 매니저 주입
        foreach (var exec in executorMap.Values)
            exec.SetManager(this);

        // 테이블 생성 (SO에서 데이터 불러오기)
        tutorialTable.CreateTable();
    }

    private void Start()
    {
        // 튜토리얼 첫 단계 실행
        GoToStep(0);
    }

    // 특정 ID의 튜토리얼 스텝 실행
    public void GoToStep(int id)
    {
        if (!tutorialTable.DataDic.TryGetValue(id, out currentStep))
        {
            Debug.Log("튜토리얼 종료");
            return;
        }

        var executor = executorMap[currentStep.ActionData.ActionType];
        executor?.Enter(currentStep.ActionData);
    }

    // 현재 스텝을 종료하고 다음 스텝으로 전환
    public void CompleteCurrentStep()
    {
        var executor = executorMap[currentStep.ActionData.ActionType];
        executor?.Exit();

        GoToStep(currentStep.NextID);
    }
}
