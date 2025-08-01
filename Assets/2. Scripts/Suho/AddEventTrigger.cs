using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class AddEventTrigger : EditorWindow
{
    [MenuItem("Tools/Add Event Trigger to Buttons")]
    public static void AddEventTriggerToButtons()
    {
        // 모든 UI 버튼 검색
        List<Button> buttons = new List<Button>(FindObjectsOfType<Button>());

        foreach (Button button in buttons)
        {
            // 이미 Event Trigger가 있는지 확인
            EventTrigger eventTrigger = button.GetComponent<EventTrigger>();
            if (eventTrigger == null)
            {
                eventTrigger = button.gameObject.AddComponent<EventTrigger>();
            }

            AddEvent(eventTrigger, EventTriggerType.PointerEnter, OnHoveringSound);
        }

        Debug.Log("모든 버튼에 이벤트 트리거 추가 완료");
    }

    // 이벤트 처리 함수 (예시)
    static void OnHoveringSound(BaseEventData eventData)
    {
        PointerEventData pointerData = eventData as PointerEventData;
        if (pointerData != null)
        {
            AudioManager.Instance.PlaySFX(SFXName.HoverUISound.ToString());
        }
    }

    // 이벤트 추가 함수
    static void AddEvent(EventTrigger trigger, EventTriggerType eventType, UnityEngine.Events.UnityAction<BaseEventData> callback)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = eventType };
        entry.callback.AddListener(callback);
        trigger.triggers.Add(entry);
    }
}