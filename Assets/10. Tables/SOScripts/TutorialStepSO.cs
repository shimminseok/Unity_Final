using UnityEngine;

public enum TutorialActionType
{
    FadeEffect,
    Dialogue,
    HighlightUI,
    TriggerWait
}

[CreateAssetMenu(fileName = "TutorialStep", menuName = "Tutorial/Step", order = 0)]
public class TutorialStepSO : ScriptableObject
{
    public int ID;
    public int NextID;

    public TutorialActionData ActionData;

    [TextArea] public string Description;
}
