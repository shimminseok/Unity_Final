using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    private readonly Dictionary<Type, UIBase> UIDict = new();
    private List<UIBase> openedUIList = new();


    private void Start()
    {
        InitializeUIRoot();
    }

    public void InitializeUIRoot()
    {
        UIDict.Clear();

        Transform uiRoot = GameObject.Find("UIRoot")?.transform;
        if (uiRoot == null)
        {
            Debug.LogWarning("[UIManager] UIRoot를 찾을 수 없습니다.");
            return;
        }

        UIBase[] uiComponents = uiRoot.GetComponentsInChildren<UIBase>(true);
        foreach (UIBase uiComponent in uiComponents)
        {
            UIDict[uiComponent.GetType()] = uiComponent;
            uiComponent.Close();
        }
    }

    public void Open<T>() where T : UIBase
    {
        if (UIDict.TryGetValue(typeof(T), out UIBase ui))
        {
            openedUIList.Add(ui);
            ui.Open();
        }
    }

    public void Close<T>() where T : UIBase
    {
        if (UIDict.TryGetValue(typeof(T), out UIBase ui) && openedUIList.Contains(ui))
        {
            openedUIList.Remove(ui);
            ui.Close();
        }
    }

    public T GetUIComponent<T>() where T : UIBase
    {
        return UIDict.TryGetValue(typeof(T), out var ui) ? ui as T : null;
    }
}

public class UIBase : MonoBehaviour
{
    [SerializeField] private RectTransform contents;

    protected RectTransform Contents => contents;

    public virtual void Open()
    {
        contents.gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        contents.gameObject.SetActive(false);
    }
}