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

    [TextArea] public string Description;
}
