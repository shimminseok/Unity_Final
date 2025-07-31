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
        {
            InitializeUIRoot();
        }
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

        UIBase[]         uiComponents = uiRoot.GetComponentsInChildren<UIBase>(true);
        List<GameObject> toDestroy    = new();
        foreach (UIBase uiComponent in uiComponents)
        {
            Type type = uiComponent.GetType();
            if (!uiDict.TryAdd(type, uiComponent))
            {
                toDestroy.Add(uiComponent.gameObject);
            }
            else
            {
                uiComponent.Close();
            }
        }


        foreach (GameObject go in toDestroy)
        {
            Destroy(go);
        }
    }

    public void Open(UIBase ui)
    {
        if (!openedUIList.Contains(ui))
        {
            openedUIList.Add(ui);
            AudioManager.Instance.PlaySFX(SFXName.OpenUISound.ToString());
        }

        ui?.Open();
    }

    public void Close(UIBase ui)
    {
        if (openedUIList.Contains(ui))
        {
            openedUIList.Remove(ui);
            AudioManager.Instance.PlaySFX(SFXName.OpenUISound.ToString());
            ui.Close();
        }
    }

    public void CloseLastOpenedUI()
    {
        if (openedUIList.Count == 0)
        {
            TwoChoicePopup popup = PopupManager.Instance.GetUIComponent<TwoChoicePopup>();
            popup.SetAndOpenPopupUI("종료하기",
                "게임을 종료하시겠습니까?",
                () =>
                {
                    SaveLoadManager.Instance.HandleApplicationQuit();
                },
                null,
                "종료하기");
            return;
        }

        UIBase ui = openedUIList.Last();
        Close(ui);
    }

    public T GetUIComponent<T>() where T : UIBase
    {
        return uiDict.TryGetValue(typeof(T), out UIBase ui) ? ui as T : null;
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

    public void OnClickCloseBtn()
    {
        UIManager.Instance.Close(this);
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
            {
                return null;
            }

            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    GameObject go = new($"[{typeof(T).Name}]");
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
        {
            instance = null;
        }
    }
}