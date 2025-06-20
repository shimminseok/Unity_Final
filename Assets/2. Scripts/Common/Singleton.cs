using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType(typeof(T)) as T;
                if (instance == null)
                {
                    SetupInstance();
                }

                DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }
    }

    protected bool IsDuplicate { get; private set; }
    protected virtual void Awake()
    {
        RemoveDuplicates();
    }

    private void RemoveDuplicates()
    {
        if (Instance == null)
            SetupInstance();
        else if (instance != null && instance != this as T)
        {
            Destroy(gameObject);
            IsDuplicate = true;
        }
    }

    private static void SetupInstance()
    {
        GameObject gameObj = new GameObject();
        gameObj.name = typeof(T).Name;
        instance = gameObj.AddComponent<T>();
    }
}