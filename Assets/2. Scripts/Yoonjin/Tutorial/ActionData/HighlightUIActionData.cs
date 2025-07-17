using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "HighlightUIAction", menuName = "Tutorial/Action/HighlightUI")]
public class HighlightUIActionData : TutorialActionData
{
    public GameObject targetButton;          // 강조할 버튼
    public bool autoBlockOthers = true;      // 나머지 버튼 자동 차단 여부
    public List<GameObject> exceptionButtons; // autoBlockOthers = false일 때 유지할 버튼
}
