using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        var tutorialData = SaveLoadManager.Instance
            .SaveDataMap.GetValueOrDefault(SaveModule.Tutorial) as SaveTutorialData;

        // 데이터가 없으면 새로 생성
        if (tutorialData == null)
        {
            tutorialData = new SaveTutorialData
            {
                Phase = TutorialPhase.DeckBuildingBefore,
                IsCompleted = false
            };

            SaveLoadManager.Instance.SaveDataMap[SaveModule.Tutorial] = tutorialData;
            SaveLoadManager.Instance.SaveModuleData(SaveModule.Tutorial);
        }

        // 튜토리얼 완료된 경우 비활성화
        if (tutorialData.IsCompleted)
        {
            IsActive = false;
            return;
        }

        IsActive = true;

        // DeckBuildingBefore 페이즈에서는 장비, 스킬, 유닛 초기화
        if (tutorialData.Phase == TutorialPhase.DeckBuildingBefore)
        {
            var unitList = AccountManager.Instance.GetPlayerUnits();

            foreach (var unit in unitList)
            {
                // 출전 중인 유닛 해제
                unit.Compete(-1, false);

                // 장비 해제
                var equippedTypes = unit.EquippedItems.Keys.ToList();
                foreach (var type in equippedTypes)
                {
                    unit.UnEquipItem(type);
                }

                // 스킬 해제
                foreach (var skill in unit.SkillDatas)
                {
                    if (skill != null)
                        unit.UnEquipSkill(skill);
                }
            }

            // 저장
            SaveLoadManager.Instance.SaveModuleData(SaveModule.InventoryUnit);
        }

        // 재시작 지점 설정
        int resumeStep = tutorialTable.DataDic.Values
            .Where(step => step.phase == tutorialData.Phase && step.isResumeEntryPoint)
            .Select(step => step.ID)
            .FirstOrDefault();

        // 유효한 ID인지 확인 (예외 처리 추가)
        if (!tutorialTable.DataDic.ContainsKey(resumeStep))
        {
            if (tutorialData.Phase == TutorialPhase.DeckBuildingBefore)
            {
                Debug.LogWarning("[튜토리얼] ResumeStep이 없어 기본 ID 0으로 시작합니다.");
                resumeStep = 0;
            }
            else
            {
                Debug.LogError($"[튜토리얼] resumeStep ID {resumeStep}이 존재하지 않습니다. 튜토리얼을 종료합니다.");
                EndTutorial();
                return;
            }
        }

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
        Debug.Log($"현재 페이즈 {currentStep.phase}");
        executor.Enter(currentStep.ActionData);
    }


    // 현재 스텝을 종료하고 다음 스텝으로 전환
    public void CompleteCurrentStep()
    {
        var executor = executorMap[currentStep.ActionData.ActionType];
        executor?.Exit();

        // 현재 페이즈 저장
        if (SaveLoadManager.Instance.SaveDataMap.GetValueOrDefault(SaveModule.Tutorial) is SaveTutorialData tutorialData)
        {
            tutorialData.Phase = currentStep.phase;
            SaveLoadManager.Instance.SaveModuleData(SaveModule.Tutorial);
        }

        GoToStep(currentStep.NextID);
    }

    // 튜토리얼 종료
    public void EndTutorial()
    {
        IsActive = false;

        if (SaveLoadManager.Instance.SaveDataMap.GetValueOrDefault(SaveModule.Tutorial) is SaveTutorialData tutorialData)
        {
            tutorialData.IsCompleted = true;

            SaveLoadManager.Instance.SaveModuleData(SaveModule.Tutorial);
        }

        Debug.LogWarning("튜토리얼 종료!");
    }

}
