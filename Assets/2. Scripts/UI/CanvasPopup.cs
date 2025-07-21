using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasPopup : Singleton<CanvasPopup>
{
    private readonly Dictionary<Type, UIBase> uiDict = new();


    protected override void Awake()
    {
        base.Awake();
        if (isDuplicated)
            return;

        InitializeUIRoot();
    }

    private void InitializeUIRoot()
    {
        uiDict.Clear();

        UIBase[]         uiComponents = GetComponentsInChildren<UIBase>(true);
        List<GameObject> toDestroy    = new();

        foreach (UIBase uiComponent in uiComponents)
        {
            var type = uiComponent.GetType();
            if (!uiDict.TryAdd(type, uiComponent))
            {
                toDestroy.Add(uiComponent.gameObject);
            }
            else
            {
                uiComponent.Close();
            }
        }

        foreach (var go in toDestroy)
            Destroy(go);
    }

    public T GetUIComponent<T>() where T : UIBase
    {
        return uiDict.TryGetValue(typeof(T), out UIBase ui) ? ui as T : null;
    }
}