using UnityEngine;

public enum TutorialActionType
{
    Dialogue,
    HighlightUI,
    TriggerWait,
    Reward
}

[CreateAssetMenu(fileName = "TutorialStep", menuName = "ScriptableObjects/Tutorial/TutorialStep", order = 0)]
public class TutorialStepSO : ScriptableObject
{
    public int ID;
    public int NextID;

    public TutorialActionData ActionData;

    public TutorialManager.TutorialPhase phase; // 이 스텝이 속한 페이즈
    public bool isResumeEntryPoint;             // 재개 시점 여부

    [TextArea] public string Description;
}
