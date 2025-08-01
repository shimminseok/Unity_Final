using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ImagePopupAction", menuName = "ScriptableObjects/Tutorial/Actions/ImagePopup", order = 1)]
public class ImagePopupActionData : TutorialActionData
{
    public string prefabAddress;
    public string parentCanvasName;

    private void OnEnable()
    {
        ActionType = TutorialActionType.ImagePopup;
    }
}

