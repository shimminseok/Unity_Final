using UnityEngine;

[CreateAssetMenu(fileName = "DialogueAction", menuName = "Tutorial/Action/Dialogue", order = 0)]
public class DialogueActionData : TutorialActionData
{
    [TextArea] public string dialogGroupKey;
}
