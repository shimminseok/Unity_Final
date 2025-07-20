using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TutorialUIHighlighter
{
    private static GameObject highlightEffect;

    public static void Highlight(GameObject target)
    {
        if (highlightEffect == null)
        {
            highlightEffect = GameObject.Instantiate(Resources.Load<GameObject>("UI/HighlightEffect"));
        }

        // 부모로 붙이기 (로컬 좌표 유지)
        highlightEffect.transform.SetParent(target.transform, false);

        // RectTransform 설정 동기화
        var rect = highlightEffect.GetComponent<RectTransform>();
        if (rect != null)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            rect.localScale = Vector3.one;
        }

        highlightEffect.transform.SetAsLastSibling(); // 가장 위로
        highlightEffect.SetActive(true);
    }

    public static void Clear()
    {
        if (highlightEffect != null)
            highlightEffect.SetActive(false);
    }
}
