using System.Collections.Generic;
using UnityEngine;

public enum TutorialActionType
{
    Dialogue,
    HighlightUI,
    TriggerWait,
    Reward,
    ImagePopup
}

[CreateAssetMenu(fileName = "TutorialStep", menuName = "ScriptableObjects/Tutorial/TutorialStep", order = 0)]
public class TutorialStepSO : ScriptableObject
{
    public int ID;
    public int NextID;
    public TutorialManager.TutorialPhase phase; // 이 스텝이 속한 페이즈
    public bool isResumeEntryPoint;             // 재개 시점 여부

    public List<TutorialActionData> Actions;    // List로 변경해 복합 실행

    [TextArea] public string Description;
}
