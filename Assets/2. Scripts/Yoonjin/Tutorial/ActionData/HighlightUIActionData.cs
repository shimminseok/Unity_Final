using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "HighlightUIAction", menuName = "ScriptableObjects/Tutorial/Actions/HighlightUI")]
public class HighlightUIActionData : TutorialActionData
{
    public string targetButtonName;          // 강조할 버튼
    public bool autoBlockOthers = true;      // 나머지 버튼 자동 차단 여부
}
