using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    [Tooltip("대사 주인공 이름")]
    public string characterName;

    [Tooltip("대사 내용 (줄바꿈 지원)")]
    [TextArea]
    public string dialogue;

    [Tooltip("초상화 리소스 키 (Resources/Portraits 폴더 내부)")]
    public string portraitKey;

    [Tooltip("배경 리소스 키 (Resources/Backgrounds 폴더 내부)")]
    public string backgroundKey;
}
