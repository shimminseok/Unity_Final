using System;
using UnityEngine;

public class VFXController : MonoBehaviour
{
    public static PoolableVFX InstantiateVFX(string poolID, GameObject prefab)
    {
        GameObject go = ObjectPoolManager.Instance.GetObject(poolID);
        if (go == null) go = Instantiate(prefab);
        PoolableVFX vfx = go.GetComponent<PoolableVFX>();
        return vfx;
    }
    
    public static void SubScribe(string poolID, GameObject prefab, Action eventTrigger)
    {
        Action handler = null;
        handler = () =>
        {
            InstantiateVFX(poolID, prefab);
            eventTrigger -= handler; // ✅ 실행 후 구독 취소
        };
        eventTrigger += handler;
    }
    
}