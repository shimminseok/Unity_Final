using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class SubtitleSignalReceiver : MonoBehaviour, INotificationReceiver
{
    [Header("Bindings")]
    public TypeWriter typeWriter;
    public TMP_Text uiText;

    void Awake()
    {
        // 자동 바인딩: 누락돼 있으면 Canvas/SubtitleText 찾기
        if (uiText == null)
            uiText = GameObject.Find("Canvas/SubtitleText")?.GetComponent<TMP_Text>();

        if (typeWriter == null && uiText != null)
            typeWriter = uiText.GetComponent<TypeWriter>();

        if (uiText == null) Debug.LogWarning("[Subtitle] uiText가 비었습니다.");
        if (typeWriter == null) Debug.LogWarning("[Subtitle] typeWriter가 비었습니다.");
    }

    public void OnNotify(Playable origin, INotification notification, object context)
    {
        var marker = notification as SubtitleSignalMarker;
        if (marker == null) return; // 다른 시그널은 무시

        if (typeWriter == null || uiText == null)
        {
            return;
        }

        if (marker.completeCurrentBeforeNew && typeWriter.IsTyping)
            typeWriter.CompleteTyping();

        if (string.IsNullOrEmpty(marker.text))
        {
            uiText.text = ""; // 클리어
            return;
        }

        if (marker.overrideSpeed) typeWriter.StartTyping(marker.text, marker.charsPerSecond);
        else typeWriter.StartTyping(marker.text, null);
    }
}