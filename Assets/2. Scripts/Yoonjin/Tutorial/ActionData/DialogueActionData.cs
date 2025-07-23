using UnityEngine;

[CreateAssetMenu(fileName = "DialogueAction", menuName = "ScriptableObjects/Tutorial/Actions/Dialogue", order = 0)]
public class DialogueActionData : TutorialActionData
{
    [TextArea] public string dialogGroupKey;

    private void OnEnable()
    {
        ActionType = TutorialActionType.Dialogue;
    }
}
