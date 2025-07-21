using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    private readonly Dictionary<Type, UIBase> uiDict = new();
    private List<UIBase> openedUIList = new();


    protected override void Awake()
    {
        base.Awake();
        if (!isDuplicated)
            InitializeUIRoot();
    }

    public void InitializeUIRoot()
    {
        uiDict.Clear();

        Transform uiRoot = GameObject.Find("UIRoot")?.transform;
        if (uiRoot == null)
        {
            Debug.LogWarning("[UIManager] UIRoot를 찾을 수 없습니다.");
            return;
        }

        UIBase[] uiComponents = uiRoot.GetComponentsInChildren<UIBase>(true);
        foreach (UIBase uiComponent in uiComponents)
        {
            if (uiDict.ContainsKey(uiComponent.GetType()))
            {
                Destroy(uiComponent.gameObject);
            }
            else
            {
                uiDict[uiComponent.GetType()] = uiComponent;
                uiComponent.Close();
            }
        }
    }

    public void Open(UIBase ui)
    {
        if (!openedUIList.Contains(ui))
            openedUIList.Add(ui);

        ui?.Open();
    }

    public void Close(UIBase ui)
    {
        if (openedUIList.Contains(ui))
        {
            openedUIList.Remove(ui);
            ui.Close();
        }
    }

    public void CloseLastOpenedUI()
    {
        if (openedUIList.Count == 0)
            return;

        UIBase ui = openedUIList.Last();
        Close(ui);
    }

    public T GetUIComponent<T>() where T : UIBase
    {
        return uiDict.TryGetValue(typeof(T), out var ui) ? ui as T : null;
    }
}

public class UIBase : MonoBehaviour
{
    [SerializeField] private RectTransform contents;

    protected RectTransform Contents  => contents;
    protected UIManager     UIManager => UIManager.Instance;

    public virtual void Open()
    {
        contents.gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        contents.gameObject.SetActive(false);
    }
}

public class SingletonUI<T> : UIBase where T : UIBase
{
    private static T instance;
    private static bool isShuttingDown;

    public static T Instance
    {
        get
        {
            if (isShuttingDown)
                return null;

            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    GameObject go = new GameObject($"[{typeof(T).Name}]");
                    instance = go.AddComponent<T>();
                    DontDestroyOnLoad(go);
                    instance.gameObject.SetActive(false);
                }
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this as T;
        DontDestroyOnLoad(gameObject);
        gameObject.SetActive(false);
    }

    protected virtual void OnApplicationQuit()
    {
        isShuttingDown = true;
        instance = null;
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }
}