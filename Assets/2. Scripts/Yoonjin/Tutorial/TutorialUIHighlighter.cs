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
            highlightEffect = GameObject.Instantiate(Resources.Load<GameObject>("HighlightEffect"));
        }

        highlightEffect.transform.SetParent(target.transform, false);
        highlightEffect.transform.SetAsLastSibling();
        highlightEffect.SetActive(true);
    }

    public static void Clear()
    {
        if (highlightEffect != null)
        {
            highlightEffect.SetActive(false);
        }
    }
}
