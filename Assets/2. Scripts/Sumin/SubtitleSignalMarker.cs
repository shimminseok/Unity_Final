using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class SubtitleSignalMarker : Marker, INotification
{
    // 고유 식별자 (Signal Emitter에서 자동으로 씀)
    public PropertyName id => new PropertyName();

    [TextArea] public string text;

    [Header("Optional Overrides")]
    public bool overrideSpeed = false;
    public float charsPerSecond = 35f;   // overrideSpeed가 true일 때만 반영

    [Header("Misc")]
    public bool completeCurrentBeforeNew = true; // 새 자막 시작 전 이전 타이핑 완성
}